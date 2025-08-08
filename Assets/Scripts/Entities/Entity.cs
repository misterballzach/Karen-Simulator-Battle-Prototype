using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    void Start()
    {
        currentHealth = maxHealth;
        ShuffleDeck();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Debug.Log($"{name} took {amount} damage, now has {currentHealth} health.");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"{name} healed for {amount}, now has {currentHealth} health.");
    }

    public void DrawCard()
    {
        if (deck.Count == 0)
        {
            // Reshuffle discard pile into deck
            foreach(Card card in discardPile)
            {
                deck.Add(card);
            }
            discardPile.Clear();
            ShuffleDeck();
        }

        if (deck.Count > 0)
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            hand.Add(card);
            Debug.Log($"{name} drew {card.name}.");
        }
        else
        {
            Debug.Log("No cards to draw.");
        }
    }

    public void PlayCard(Card card, Entity target)
    {
        if (hand.Contains(card))
        {
            card.Use(target);
            hand.Remove(card);
            discardPile.Add(card);
            Debug.Log($"{name} played {card.name}.");
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        effect.Apply(this);
    }

    public void OnTurnStart()
    {
        foreach (var effect in statusEffects)
        {
            effect.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        List<StatusEffect> effectsToRemove = new List<StatusEffect>();
        foreach (var effect in statusEffects)
        {
            effect.OnTurnEnd();
            if (effect.duration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        foreach (var effect in effectsToRemove)
        {
            effect.Remove();
            statusEffects.Remove(effect);
        }
    }
}
