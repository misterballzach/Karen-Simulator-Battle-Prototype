using UnityEngine;
using System.Collections.Generic;

public class CardRewardSystem : MonoBehaviour
{
    public List<Card> rewardableCards;
    public CardRewardUI rewardUI;
    public Entity player;

    public void ShowRewardScreen(int numberOfChoices)
    {
        List<Card> choices = new List<Card>();
        List<Card> availableCards = new List<Card>(rewardableCards);

        for (int i = 0; i < numberOfChoices; i++)
        {
            if (availableCards.Count == 0) break;

            int randomIndex = Random.Range(0, availableCards.Count);
            choices.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex);
        }

        rewardUI.DisplayRewardChoices(choices);
    }

    public void OnCardChosen(Card chosenCard)
    {
        if (player != null && chosenCard != null)
        {
            // A real implementation would add this to a master deck list,
            // not the temporary battle deck.
            player.deck.Add(chosenCard);
            Debug.Log($"Added {chosenCard.name} to the player's deck.");
        }
        rewardUI.HideRewardScreen();
    }
}
