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
        foreach (Card card in PlayerProfile.s_instance.masterCardCollection)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, collectionContainer);
            // Setup card display
            // Add button listener to add card to deck
            cardObj.GetComponentInChildren<Text>().text = card.name;
            cardObj.GetComponent<Button>().onClick.AddListener(() => AddCardToDeck(card));
        }
    }

    void PopulateDeck()
    {
        // Clear container
        foreach (Transform child in deckContainer) { Destroy(child.gameObject); }

        // Fill with cards from player profile
        foreach (Card card in PlayerProfile.s_instance.currentDeck)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, deckContainer);
            // Setup card display
            // Add button listener to remove card from deck
            cardObj.GetComponentInChildren<Text>().text = card.name;
            cardObj.GetComponent<Button>().onClick.AddListener(() => RemoveCardFromDeck(card));
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (PlayerProfile.s_instance.currentDeck.Count >= MAX_DECK_SIZE)
        {
            Debug.Log("Deck is full!");
            return;
        }
        PlayerProfile.s_instance.currentDeck.Add(card);
        PopulateDeck();
    }

    public void RemoveCardFromDeck(Card card)
    {
        PlayerProfile.s_instance.currentDeck.Remove(card);
        PopulateDeck();
    }
}
