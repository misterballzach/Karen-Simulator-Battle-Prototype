using UnityEngine;
using System.Collections;

public enum EncounterState { Start, PlayerTurn, EnemyTurn, Won, Lost, Reward }

using System.Collections.Generic;

public class Encounter : MonoBehaviour
{
    public Combatant player;
    public Combatant enemy;
    public Location currentLocation;

    public ArgumentHandUI playerHandUI;
    public ConsequenceList consequenceList;

    private Dictionary<VerbalAbility, int> playerCooldowns = new Dictionary<VerbalAbility, int>();
    private Dictionary<VerbalAbility, int> enemyCooldowns = new Dictionary<VerbalAbility, int>();

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
        ApplyLocationEffect();

        if (PlayerProfile.s_instance != null)
        {
            player.verbalLoadout = new System.Collections.Generic.List<VerbalAbility>(PlayerProfile.s_instance.currentDeck);
            player.ShuffleLoadout();
        }

        for(int i = 0; i < 3; i++)
        {
            player.PrepareArgument();
            enemy.PrepareArgument();
        }
        playerHandUI.UpdateHandUI();

        yield return new WaitForSeconds(1f);

        state = EncounterState.PlayerTurn;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        UpdateCooldowns(playerCooldowns);
        player.OnTurnStart();
        if (!player.CanTakeTurn())
        {
            StartCoroutine(EndPlayerTurn());
            return;
        }
        playerHandUI.UpdateHandUI();
        Debug.Log("Player's turn.");
    }

    public void OnAbilityUsed(VerbalAbility ability, Combatant target)
    {
        if (state != EncounterState.PlayerTurn) return;
        if (playerCooldowns.ContainsKey(ability))
        {
            Debug.Log($"{ability.name} is on cooldown for {playerCooldowns[ability]} more turns.");
            return;
        }

        if (player.UseAbility(ability, target))
        {
            ApplyReputationModifiers(ability);
            TriggerMeterGains(player, ability);
            CheckEscalationRisk(player, ability);

            if (ability.cooldown > 0)
            {
                playerCooldowns[ability] = ability.cooldown;
            }
            playerHandUI.UpdateHandUI();
            StartCoroutine(EndPlayerTurn());
        }
        else
        {
            Debug.Log("Player failed to use ability.");
        }
    }

    public void OnEndTurnButton()
    {
        if (state != EncounterState.PlayerTurn) return;
        StartCoroutine(EndPlayerTurn());
    }

    IEnumerator EndPlayerTurn()
    {
        player.OnTurnEnd();
        yield return new WaitForSeconds(1f);

        if (enemy.currentEmotionalStamina <= 0 || enemy.insight >= enemy.maxInsight)
        {
            state = EncounterState.Won;
            EndEncounter();
        }
        else
        {
            state = EncounterState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        UpdateCooldowns(enemyCooldowns);
        enemy.OnTurnStart();
        if (!enemy.CanTakeTurn())
        {
            enemy.OnTurnEnd();
            if (player.currentEmotionalStamina <= 0) { state = EncounterState.Lost; EndEncounter(); }
            else { state = EncounterState.PlayerTurn; StartPlayerTurn(); }
            yield break;
        }
        Debug.Log("Enemy's turn.");

        yield return new WaitForSeconds(1f);

        Tuple<VerbalAbility, Combatant> aiChoice = null;
        if (enemy.aiProfile != null)
        {
            aiChoice = enemy.aiProfile.ChooseAbility(enemy, player, enemy.preparedArguments, enemyCooldowns);
        }
        else
        {
            Debug.LogWarning("Enemy has no AI Profile assigned!");
        }

        if (aiChoice != null && aiChoice.Item1 != null)
        {
            VerbalAbility bestAbility = aiChoice.Item1;
            Combatant target = aiChoice.Item2;

            ApplyReputationModifiers(bestAbility);
            TriggerMeterGains(enemy, bestAbility);
            CheckEscalationRisk(enemy, bestAbility);

            if (bestAbility.cooldown > 0)
            {
                enemyCooldowns[bestAbility] = bestAbility.cooldown;
            }

            enemy.UseAbility(bestAbility, target);
            Debug.Log($"Enemy chose to use {bestAbility.name} on {target.name}.");
        }
        else
        {
            Debug.Log("Enemy has no good plays, ending turn.");
        }

        yield return new WaitForSeconds(1f);

        enemy.OnTurnEnd();

        if (player.currentEmotionalStamina <= 0 || player.insight >= player.maxInsight)
        {
            state = EncounterState.Lost;
            EndEncounter();
        }
        else
        {
            state = EncounterState.PlayerTurn;
            StartPlayerTurn();
        }
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
            bool wasLiberated = enemy.insight >= enemy.maxInsight;
            string winMessage = wasLiberated ? "You've liberated them with a stunning emotional breakthrough!" : "You won the argument!";
            Debug.Log(winMessage);

            if (wasLiberated)
            {
                CommuneManager.s_instance?.AddNewMember(enemy);
                ReputationManager.s_instance?.AddReputation(enemy.faction, 5); // Small bonus for liberating
            }
            else
            {
                ReputationManager.s_instance?.AddReputation(enemy.faction, -10); // Penalty for dominating
            }

            int xpGained = 50;
            if (isViral)
            {
                Debug.Log("Your meltdown went viral! Consequences are doubled!");
                xpGained *= 2;
            }
            player.GainXP(xpGained);

            state = EncounterState.Reward;
            Debug.Log("Starting reward phase...");
        }
        else if (state == EncounterState.Lost)
        {
            string loseMessage = player.insight >= player.maxInsight ? "You had an emotional breakthrough and lost the argument." : "You had a breakdown.";
            Debug.Log(loseMessage);
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
        if (combatant == player)
        {
            playerCooldowns.Clear();
            Debug.Log("Player cooldowns cleared!");
        }
        else if (combatant == enemy)
        {
            enemyCooldowns.Clear();
            Debug.Log("Enemy cooldowns cleared!");
        }
    }

    void CheckEscalationRisk(Combatant user, VerbalAbility ability)
    {
        if (ability.escalationRisk > 0)
        {
            if (Random.value < ability.escalationRisk)
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
