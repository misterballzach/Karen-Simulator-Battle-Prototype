using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum EncounterState { Start, PlayerTurn, EnemyTurn, Won, Lost, Reward }

public class Encounter : MonoBehaviour
{
    public List<Combatant> playerParty;
    public List<Combatant> enemyParty;
    public Location currentLocation;

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;

    public ArgumentHandUI playerHandUI;
    public ConsequenceList consequenceList;

    // We'll need separate cooldown dictionaries for each combatant.
    // For simplicity in this step, we'll keep it as one for the party for now.
    // This will be addressed properly when updating turn management.
    private Dictionary<VerbalAbility, int> playerCooldowns = new Dictionary<VerbalAbility, int>();
    private Dictionary<VerbalAbility, int> enemyCooldowns = new Dictionary<VerbalAbility, int>();

    public Combatant ActiveCombatant => state == EncounterState.PlayerTurn ? playerParty[currentPlayerIndex] : enemyParty[currentEnemyIndex];

    [HideInInspector]
    public bool isViral = false;

    public EncounterState state;

    void Start()
    {
        state = EncounterState.Start;
        StartCoroutine(SetupEncounter());
    }

    IEnumerator SetupEncounter()
    {
        // Set the encounter reference for all combatants
        foreach (var combatant in playerParty) combatant.currentEncounter = this;
        foreach (var combatant in enemyParty) combatant.currentEncounter = this;

        ApplyLocationEffect();

        // Assume the first player is the main character and gets the deck from the profile
        if (PlayerProfile.s_instance != null && playerParty.Count > 0)
        {
            playerParty[0].verbalLoadout = new System.Collections.Generic.List<VerbalAbility>(PlayerProfile.s_instance.currentDeck);
            playerParty[0].ShuffleLoadout();
        }

        // Each combatant prepares their starting hand
        foreach (var combatant in playerParty)
        {
            for (int i = 0; i < 3; i++) combatant.PrepareArgument();
        }
        foreach (var combatant in enemyParty)
        {
            for (int i = 0; i < 3; i++) combatant.PrepareArgument();
        }

        playerHandUI.UpdateHandUI(); // This will need to be updated to show the active player's hand

        yield return new WaitForSeconds(1f);

        state = EncounterState.PlayerTurn;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        // To-do: This needs to be per-combatant
        UpdateCooldowns(playerCooldowns);

        ActiveCombatant.OnTurnStart();
        if (!ActiveCombatant.CanTakeTurn())
        {
            Debug.Log($"{ActiveCombatant.name} cannot take their turn.");
            StartCoroutine(EndPlayerTurn());
            return;
        }

        // The UI needs to be aware of which player is active
        // playerHandUI.SetTarget(ActiveCombatant); // Imagined method
        playerHandUI.UpdateHandUI();
        Debug.Log($"{ActiveCombatant.name}'s turn.");
    }

    public void OnAbilityUsed(VerbalAbility ability, Combatant target)
    {
        if (state != EncounterState.PlayerTurn) return;
        if (playerCooldowns.ContainsKey(ability))
        {
            Debug.Log($"{ability.name} is on cooldown for {playerCooldowns[ability]} more turns.");
            return;
        }

        var user = ActiveCombatant;
        if (user.UseAbility(ability, target))
        {
            // --- Post-ability Triggers ---
            // Handle Petty Solidarity passive
            if (user.karenClass == KarenClass.RepentantKaren && ability.isDebuff)
            {
                Debug.Log("Petty Solidarity triggers! The party gains morale.");
                foreach (var ally in playerParty)
                {
                    ally.RecoverStamina(5);
                }
            }

            ApplyReputationModifiers(ability);
            TriggerMeterGains(user, ability);
            CheckEscalationRisk(user, ability);

            if (ability.cooldown > 0)
            {
                playerCooldowns[ability] = ability.cooldown;
            }
            playerHandUI.UpdateHandUI();
            StartCoroutine(EndPlayerTurn());
        }
        else
        {
            Debug.Log($"{user.name} failed to use ability.");
        }
    }

    public void OnEndTurnButton()
    {
        if (state != EncounterState.PlayerTurn) return;
        StartCoroutine(EndPlayerTurn());
    }

    IEnumerator EndPlayerTurn()
    {
        ActiveCombatant.OnTurnEnd();
        yield return new WaitForSeconds(1f);

        // Check for win condition
        if (enemyParty.All(c => c.currentEmotionalStamina <= 0))
        {
            state = EncounterState.Won;
            EndEncounter();
            yield break;
        }

        currentPlayerIndex++;
        if (currentPlayerIndex >= playerParty.Count)
        {
            // All players have had their turn
            currentPlayerIndex = 0;
            state = EncounterState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            // Next player's turn
            state = EncounterState.PlayerTurn;
            StartPlayerTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy phase starts.");
        foreach (var enemy in enemyParty)
        {
            if (!enemy.CanTakeTurn())
            {
                Debug.Log($"{enemy.name} cannot take a turn.");
                continue;
            }

            enemy.OnTurnStart();
            Debug.Log($"{enemy.name}'s turn.");

            // AI chooses ability and target.
            // This needs a refactor of AIProfile to handle target selection from a party.
            // For now, we'll have it target the first player.
            Tuple<VerbalAbility, Combatant> aiChoice = null;
            if (enemy.aiProfile != null && playerParty.Count > 0)
            {
                aiChoice = enemy.aiProfile.ChooseAbility(enemy, enemyParty, playerParty, enemy.preparedArguments, enemyCooldowns);
            }
            else
            {
                Debug.LogWarning($"{enemy.name} has no AI Profile or there are no players to target!");
            }

            if (aiChoice != null && aiChoice.Item1 != null)
            {
                VerbalAbility bestAbility = aiChoice.Item1;
                Combatant target = aiChoice.Item2; // AI returns its chosen target

                ApplyReputationModifiers(bestAbility);
                TriggerMeterGains(enemy, bestAbility);
                CheckEscalationRisk(enemy, bestAbility);

                if (bestAbility.cooldown > 0)
                {
                    enemyCooldowns[bestAbility] = bestAbility.cooldown;
                }

                enemy.UseAbility(bestAbility, target);
                Debug.Log($"{enemy.name} used {bestAbility.name} on {target.name}.");
            }
            else
            {
                Debug.Log($"{enemy.name} has no good plays, ending turn.");
            }

            enemy.OnTurnEnd();

            // Check for lose condition after each enemy action
            if (playerParty.All(c => c.currentEmotionalStamina <= 0))
            {
                state = EncounterState.Lost;
                EndEncounter();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Enemy phase ends.");
        state = EncounterState.PlayerTurn;
        StartPlayerTurn();
    }

    void ApplyLocationEffect()
    {
        if (currentLocation == null || currentLocation.locationEffects == null) return;

        Debug.Log($"Battle takes place at: {currentLocation.locationName}. Applying location effects.");
        foreach (var effect in currentLocation.locationEffects)
        {
            effect.Apply(this);
        }
    }

    void UnapplyLocationEffects()
    {
        if (currentLocation == null || currentLocation.locationEffects == null) return;

        foreach (var effect in currentLocation.locationEffects)
        {
            effect.Unapply(this);
        }
    }

    void EndEncounter()
    {
        UnapplyLocationEffects(); // Unapply effects at the end of the encounter

        if (state == EncounterState.Won)
        {
            Debug.Log("Player party won the encounter!");
            // For now, base liberation/reputation on the state of the first enemy
            var enemyLeader = enemyParty.FirstOrDefault();
            if (enemyLeader != null)
            {
                bool wasLiberated = enemyLeader.insight >= enemyLeader.maxInsight;
                if (wasLiberated)
                {
                    Debug.Log("You've liberated the enemy leader!");
                    CommuneManager.s_instance?.AddNewMember(enemyLeader);
                    ReputationManager.s_instance?.AddReputation(enemyLeader.faction, 5);
                }
                else
                {
                    ReputationManager.s_instance?.AddReputation(enemyLeader.faction, -10);
                }
            }

            int xpGained = 50;
            if (isViral)
            {
                Debug.Log("Your meltdown went viral! Consequences are doubled!");
                xpGained *= 2;
            }
            foreach (var member in playerParty)
            {
                member.GainXP(xpGained);
            }

            state = EncounterState.Reward;
            Debug.Log("Starting reward phase...");
        }
        else if (state == EncounterState.Lost)
        {
            Debug.Log("Player party was defeated.");
        }
    }

    void TriggerMeterGains(Combatant user, VerbalAbility ability)
    {
        int amount = 10; // Default amount
        switch (ability.rhetoricalClass)
        {
            case RhetoricalClass.Aggression:
            case RhetoricalClass.Manipulation:
            case RhetoricalClass.Delusion:
                user.GainEntitlement(amount);
                break;
            case RhetoricalClass.Vulnerability:
                user.GainInsight(amount);
                break;
        }
    }

    void ApplyReputationModifiers(VerbalAbility ability)
    {
        if (ReputationManager.s_instance == null || ability.reputationModifiers == null) return;

        foreach (var modifier in ability.reputationModifiers)
        {
            int amount = isViral ? modifier.amount * 2 : modifier.amount;
            ReputationManager.s_instance.AddReputation(modifier.faction, amount);
        }
    }

    void UpdateCooldowns(Dictionary<VerbalAbility, int> cooldowns)
    {
        List<VerbalAbility> abilitiesOffCooldown = new List<VerbalAbility>();
        foreach (var item in cooldowns)
        {
            cooldowns[item.Key]--;
            if (cooldowns[item.Key] <= 0)
            {
                abilitiesOffCooldown.Add(item.Key);
            }
        }
        foreach (var ability in abilitiesOffCooldown)
        {
            cooldowns.Remove(ability);
        }
    }

    public void ClearCooldowns(Combatant combatant)
    {
        // TODO: Refactor cooldowns to be per-combatant
        if (playerParty.Contains(combatant))
        {
            playerCooldowns.Clear();
            Debug.Log("Player party cooldowns cleared!");
        }
        else if (enemyParty.Contains(combatant))
        {
            enemyCooldowns.Clear();
            Debug.Log("Enemy party cooldowns cleared!");
        }
    }

    void CheckEscalationRisk(Combatant user, VerbalAbility ability)
    {
        if (ability.escalationRisk > 0)
        {
            if (UnityEngine.Random.value < ability.escalationRisk)
            {
                Consequence consequence = consequenceList?.GetRandomConsequence();
                if (consequence != null)
                {
                    consequence.Trigger(user);
                }
            }
        }
    }
}
