using UnityEngine;
using System.Collections.Generic;

public class ArgumentHandUI : MonoBehaviour
{
    public GameObject abilityPrefab;
    public Combatant player;
    public Transform handContainer;

    private List<GameObject> abilityObjects = new List<GameObject>();

    public void UpdateHandUI()
    {
        foreach (GameObject abilityObj in abilityObjects)
        {
            Destroy(abilityObj);
        }
        abilityObjects.Clear();

        if (player != null && player.preparedArguments != null)
        {
            foreach (VerbalAbility abilityData in player.preparedArguments)
            {
                GameObject newAbilityObj = Instantiate(abilityPrefab, handContainer);
                newAbilityObj.name = abilityData.name;
                // A 'AbilityDisplay' script on the prefab would set visual details.
                abilityObjects.Add(newAbilityObj);
            }
        }
    }
}
