using UnityEngine;
using System.Collections.Generic;

public class CardRewardSystem : MonoBehaviour
{
    public List<VerbalAbility> rewardableCards;
    public CardRewardUI rewardUI;
    public Combatant player;

    public void ShowRewardScreen(int numberOfChoices)
    {
        List<VerbalAbility> choices = new List<VerbalAbility>();
        List<VerbalAbility> availableCards = new List<VerbalAbility>(rewardableCards);

        for (int i = 0; i < numberOfChoices; i++)
        {
            if (availableCards.Count == 0) break;

            int randomIndex = Random.Range(0, availableCards.Count);
            choices.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex);
        }

        rewardUI.DisplayRewardChoices(choices);
    }

    public void OnCardChosen(VerbalAbility chosenAbility)
    {
        if (player != null && chosenAbility != null)
        {
            // A real implementation would add this to a master deck list,
            // not the temporary battle deck.
            player.verbalLoadout.Add(chosenAbility);
            Debug.Log($"Added {chosenAbility.name} to the player's deck.");
        }
        rewardUI.HideRewardScreen();
    }
}
