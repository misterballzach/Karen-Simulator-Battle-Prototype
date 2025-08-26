using UnityEngine;
using System.Collections.Generic;

public class CustomHandUIUpdater : MonoBehaviour
{
    private ArgumentHandUI handUI;
    private Combatant player;
    private Encounter encounter;
    private GameObject cardTemplate;

    public void Setup(ArgumentHandUI handUI, Combatant player, Encounter encounter, GameObject cardTemplate)
    {
        this.handUI = handUI;
        this.player = player;
        this.encounter = encounter;
        this.cardTemplate = cardTemplate;
    }

    void Update()
    {
        // A simple way to check if the hand has changed
        if (transform.childCount != player.preparedArguments.Count)
        {
            UpdateHand();
        }
    }

    void UpdateHand()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (VerbalAbility ability in player.preparedArguments)
        {
            GameObject cardGO = Instantiate(cardTemplate, transform);
            cardGO.SetActive(true);
            cardGO.GetComponent<AbilityCardUI>().Setup(ability, player, encounter);
        }
    }
}
