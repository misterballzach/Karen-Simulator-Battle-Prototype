using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckBuilderUI : MonoBehaviour
{
    public Transform collectionContainer;
    public Transform deckContainer;
    public GameObject cardUIPrefab; // A prefab to display a card in the UI

    public const int MAX_DECK_SIZE = 30;

    void Start()
    {
        PopulateCollection();
        PopulateDeck();
    }

    void PopulateCollection()
    {
        // Clear container
        foreach (Transform child in collectionContainer) { Destroy(child.gameObject); }

        // Fill with cards from player profile
        foreach (VerbalAbility ability in PlayerProfile.s_instance.masterAbilityCollection)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, collectionContainer);
            // Setup card display
            // Add button listener to add card to deck
            cardObj.GetComponentInChildren<Text>().text = ability.name;
            cardObj.GetComponent<Button>().onClick.AddListener(() => AddCardToDeck(ability));
        }
    }

    void PopulateDeck()
    {
        // Clear container
        foreach (Transform child in deckContainer) { Destroy(child.gameObject); }

        // Fill with cards from player profile
        foreach (VerbalAbility ability in PlayerProfile.s_instance.currentDeck)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, deckContainer);
            // Setup card display
            // Add button listener to remove card from deck
            cardObj.GetComponentInChildren<Text>().text = ability.name;
            cardObj.GetComponent<Button>().onClick.AddListener(() => RemoveCardFromDeck(ability));
        }
    }

    public void AddCardToDeck(VerbalAbility ability)
    {
        if (PlayerProfile.s_instance.currentDeck.Count >= MAX_DECK_SIZE)
        {
            Debug.Log("Deck is full!");
            return;
        }
        PlayerProfile.s_instance.currentDeck.Add(ability);
        PopulateDeck();
    }

    public void RemoveCardFromDeck(VerbalAbility ability)
    {
        PlayerProfile.s_instance.currentDeck.Remove(ability);
        PopulateDeck();
    }
}
