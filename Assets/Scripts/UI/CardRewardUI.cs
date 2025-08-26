using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardRewardUI : MonoBehaviour
{
    public GameObject rewardScreen;
    public Transform choiceContainer;
    public GameObject cardChoicePrefab; // A button prefab to display a card choice
    public CardRewardSystem rewardSystem;

    private List<GameObject> choiceObjects = new List<GameObject>();

    public void DisplayRewardChoices(List<VerbalAbility> choices)
    {
        rewardScreen.SetActive(true);

        // Clear previous choices
        foreach (GameObject choiceObj in choiceObjects)
        {
            Destroy(choiceObj);
        }
        choiceObjects.Clear();

        // Create new choice buttons
        foreach (VerbalAbility ability in choices)
        {
            GameObject newChoiceObj = Instantiate(cardChoicePrefab, choiceContainer);
            // newChoiceObj.GetComponent<CardDisplay>().SetCard(ability); // Set visuals
            newChoiceObj.GetComponentInChildren<Text>().text = ability.name; // Simple display

            Button button = newChoiceObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnChoiceClicked(ability));

            choiceObjects.Add(newChoiceObj);
        }
    }

    private void OnChoiceClicked(VerbalAbility chosenAbility)
    {
        rewardSystem.OnCardChosen(chosenAbility);
    }

    public void HideRewardScreen()
    {
        rewardScreen.SetActive(false);
    }
}
