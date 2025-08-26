using UnityEngine;
using System.Collections.Generic;

public class DemoArgumentHandUI : MonoBehaviour
{
    public GameObject cardPrefabTemplate; // Set by the generator

    private Combatant player;
    private Encounter encounter;
    private List<GameObject> abilityObjects = new List<GameObject>();

    public void Setup(Combatant player, Encounter encounter)
    {
        this.player = player;
        this.encounter = encounter;
    }

    public void UpdateHand()
    {
        if (player == null || encounter == null || cardPrefabTemplate == null)
        {
            Debug.LogWarning("DemoArgumentHandUI not fully set up, skipping update.");
            return;
        }

        // Clear existing cards
        foreach (GameObject abilityObj in abilityObjects)
        {
            Destroy(abilityObj);
        }
        abilityObjects.Clear();

        // Create new cards
        if (player.preparedArguments != null)
        {
            foreach (VerbalAbility abilityData in player.preparedArguments)
            {
                GameObject newAbilityObj = Instantiate(cardPrefabTemplate, transform);
                newAbilityObj.GetComponent<AbilityCardUI>().Setup(abilityData, player, encounter);
                newAbilityObj.SetActive(true);
                abilityObjects.Add(newAbilityObj);
            }
        }
    }
}
