using UnityEngine;
using System.Collections.Generic;

public class CardHandUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public Entity player;
    public Transform handContainer;

    private List<GameObject> cardObjects = new List<GameObject>();

    public void UpdateHandUI()
    {
        // Clear existing cards
        foreach (GameObject cardObj in cardObjects)
        {
            Destroy(cardObj);
        }
        cardObjects.Clear();

        // Create new cards
        if (player != null && player.hand != null)
        {
            for (int i = 0; i < player.hand.Count; i++)
            {
                Card cardData = player.hand[i];
                GameObject newCardObj = Instantiate(cardPrefab, handContainer);
                newCardObj.name = cardData.name;
                // A 'CardDisplay' script on the prefab would set visual details.
                // For now, we just instantiate the prefab.
                cardObjects.Add(newCardObj);
            }
        }
    }
}
