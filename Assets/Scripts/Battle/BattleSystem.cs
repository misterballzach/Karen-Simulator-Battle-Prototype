using UnityEngine;
using System.Collections;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost, Reward }

public class BattleSystem : MonoBehaviour
{
    public Entity player;
    public Entity enemy;
    public Location currentLocation;

    public CardHandUI playerHandUI;
    // public CardRewardSystem cardRewardSystem; // To be added

    public BattleState state;

    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        ApplyLocationEffect();

        // Load player's deck from profile
        if (PlayerProfile.s_instance != null)
        {
            player.deck = new List<Card>(PlayerProfile.s_instance.currentDeck);
            player.ShuffleDeck();
        }

        // Draw initial hands
        for(int i = 0; i < 3; i++)
        {
            player.DrawCard();
            enemy.DrawCard();
        }
        playerHandUI.UpdateHandUI();

        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        player.OnTurnStart();
        if (!player.CanTakeTurn())
        {
            StartCoroutine(EndPlayerTurn());
            return;
        }
        playerHandUI.UpdateHandUI();
        Debug.Log("Player's turn.");
    }

    public void OnCardPlayed(Card card, Entity target)
    {
        if (state != BattleState.PlayerTurn)
            return;

        if (player.PlayCard(card, target))
        {
            playerHandUI.UpdateHandUI();
            StartCoroutine(EndPlayerTurn());
        }
        else
        {
            Debug.Log("Player failed to play card.");
        }
    }

    public void OnEndTurnButton()
    {
        if (state != BattleState.PlayerTurn)
            return;

        StartCoroutine(EndPlayerTurn());
    }

    IEnumerator EndPlayerTurn()
    {
        player.OnTurnEnd();

        yield return new WaitForSeconds(1f);

        if (enemy.currentHealth <= 0)
        {
            state = BattleState.Won;
            EndBattle();
        }
        else
        {
            state = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        enemy.OnTurnStart();
        if (!enemy.CanTakeTurn())
        {
            enemy.OnTurnEnd();
            if (player.currentHealth <= 0) { state = BattleState.Lost; EndBattle(); }
            else { state = BattleState.PlayerTurn; StartPlayerTurn(); }
            yield break;
        }
        Debug.Log("Enemy's turn.");

        yield return new WaitForSeconds(1f);

        // Smarter AI logic
        Card bestCardToPlay = null;
        float bestScore = -1f;

        foreach (Card card in enemy.hand)
        {
            if (enemy.currentMana < card.cost)
            {
                continue; // Skip card if not enough mana
            }

            float currentScore = 0f;

            if (card is DamageDealCard damageCard)
            {
                currentScore = damageCard.damageAmount;
                if (player.elementalWeaknesses.Contains(damageCard.element)) currentScore *= 2f;
                if (player.elementalResistances.Contains(damageCard.element)) currentScore *= 0.5f;
                if (damageCard.damageAmount >= player.currentHealth) currentScore += 1000;
            }
            else if (card is HealingCard healingCard)
            {
                currentScore = healingCard.healAmount;
                if ((float)enemy.currentHealth / enemy.maxHealth < 0.5f) currentScore *= 2f;
            }
            else if (card is StatusEffectCard statusCard && statusCard.effectToApply.type == StatusEffectType.Debuff)
            {
                bool alreadyHasEffect = player.statusEffects.Exists(e => e.GetType() == statusCard.effectToApply.GetType());
                if (!alreadyHasEffect) currentScore = 10;
            }
            else if (card is DrawCardCard)
            {
                currentScore = 1;
            }

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestCardToPlay = card;
            }
        }

        if (bestCardToPlay != null)
        {
            Entity target = player; // Default target
            if (bestCardToPlay is HealingCard || bestCardToPlay is DrawCardCard || (bestCardToPlay is StatusEffectCard seCard && seCard.effectToApply.type == StatusEffectType.Buff))
            {
                target = enemy;
            }
            enemy.PlayCard(bestCardToPlay, target);
            Debug.Log($"Enemy chose to play {bestCardToPlay.name} with a score of {bestScore}.");
        }
        else
        {
            Debug.Log("Enemy has no good plays, ending turn.");
        }

        yield return new WaitForSeconds(1f);

        enemy.OnTurnEnd();

        if (player.currentHealth <= 0)
        {
            state = BattleState.Lost;
            EndBattle();
        }
        else
        {
            state = BattleState.PlayerTurn;
            StartPlayerTurn();
        }
    }

    void ApplyLocationEffect()
    {
        if (currentLocation == null) return;

        Debug.Log($"Battle takes place at: {currentLocation.locationName}. Applying location effects.");
        switch (currentLocation.effectType)
        {
            case LocationEffectType.ManaCostModification:
                player.manaCostModifier += currentLocation.effectValue;
                enemy.manaCostModifier += currentLocation.effectValue;
                break;
            case LocationEffectType.StartingArmor:
                player.armor += currentLocation.effectValue;
                enemy.armor += currentLocation.effectValue;
                break;
        }
    }

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("You won the battle!");
            player.GainXP(50); // Award XP

            // Start Reward Phase
            state = BattleState.Reward;
            // cardRewardSystem.ShowRewardScreen(); // This would be called here
            Debug.Log("Starting reward phase...");
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("You were defeated.");
        }
    }
}
