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

        if (enemy.currentEmotionalStamina <= 0)
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

        VerbalAbility bestAbility = null;
        float bestScore = -1f;

        foreach (VerbalAbility ability in enemy.preparedArguments)
        {
            if (enemyCooldowns.ContainsKey(ability)) continue;

            int finalCost = Mathf.Max(0, ability.cost + enemy.credibilityCostModifier);
            if (enemy.currentCredibility < finalCost) continue;

            float currentScore = 0f;
            // This AI logic needs to be updated with the new ability subclasses
            // For now, it will just play the first possible card.
            currentScore = 1;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestAbility = ability;
            }
        }

        if (bestAbility != null)
        {
            ApplyReputationModifiers(bestAbility);
            if (bestAbility.cooldown > 0)
            {
                enemyCooldowns[bestAbility] = bestAbility.cooldown;
            }
            Combatant target = player;
            // Targeting logic would go here
            enemy.UseAbility(bestAbility, target);
            Debug.Log($"Enemy chose to use {bestAbility.name}.");
        }
        else
        {
            Debug.Log("Enemy has no good plays, ending turn.");
        }

        yield return new WaitForSeconds(1f);

        enemy.OnTurnEnd();

        if (player.currentEmotionalStamina <= 0)
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
            Debug.Log("You won the argument!");
            int xpGained = 50;
            if (isViral)
            {
                Debug.Log("Your meltdown went viral! Consequences are doubled!");
                xpGained *= 2;
            }
            player.GainXP(xpGained);

            // Simple reputation change for winning
            ReputationManager.s_instance?.AddReputation(enemy.faction, -10);

            state = EncounterState.Reward;
            Debug.Log("Starting reward phase...");
        }
        else if (state == EncounterState.Lost)
        {
            Debug.Log("You had a breakdown.");
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
}
