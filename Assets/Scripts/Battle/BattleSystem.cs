using UnityEngine;
using System.Collections;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    public Entity player;
    public Entity enemy;

    public BattleState state;

    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Draw initial hands
        for(int i = 0; i < 3; i++)
        {
            player.DrawCard();
            enemy.DrawCard();
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        player.OnTurnStart();
        Debug.Log("Player's turn.");
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
        Debug.Log("Enemy's turn.");

        yield return new WaitForSeconds(1f);

        // Simple AI: find the first playable card and play it
        Card cardToPlay = null;
        foreach (Card card in enemy.hand)
        {
            if (enemy.currentMana >= card.cost)
            {
                cardToPlay = card;
                break;
            }
        }

        if (cardToPlay != null)
        {
            enemy.PlayCard(cardToPlay, player);
        }
        else
        {
            Debug.Log("Enemy has no playable cards.");
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

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("You won the battle!");
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("You were defeated.");
        }
    }
}
