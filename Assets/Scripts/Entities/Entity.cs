using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    public int level = 1;
    public int experiencePoints = 0;
    public int xpToNextLevel = 100;
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 100;
    public int currentMana;

    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public List<Element> elementalWeaknesses = new List<Element>();
    public List<Element> elementalResistances = new List<Element>();

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        ShuffleDeck();
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Element.Neutral);
    }

    public void TakeDamage(int amount, Element element)
    {
        float damageMultiplier = 1f;
        if (elementalWeaknesses.Contains(element))
        {
            damageMultiplier = 2f;
            Debug.Log($"{name} is weak to {element}!");
        }
        else if (elementalResistances.Contains(element))
        {
            damageMultiplier = 0.5f;
            Debug.Log($"{name} is resistant to {element}!");
        }

        int totalDamage = Mathf.RoundToInt(amount * damageMultiplier);

        currentHealth -= totalDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Debug.Log($"{name} took {totalDamage} damage, now has {currentHealth} health.");
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

    public bool PlayCard(Card card, Entity target)
    {
        if (hand.Contains(card) && currentMana >= card.cost)
        {
            currentMana -= card.cost;
            card.Use(this, target);
            hand.Remove(card);
            discardPile.Add(card);
            Debug.Log($"{name} played {card.name} for {card.cost} mana.");
            return true;
        }
        else
        {
            Debug.Log($"Not enough mana to play {card.name}.");
            return false;
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
        // Regenerate mana
        currentMana += 10;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        Debug.Log($"{name} regenerated 10 mana, now has {currentMana} mana.");

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

    public void GainXP(int amount)
    {
        experiencePoints += amount;
        Debug.Log($"{name} gained {amount} XP!");
        if (experiencePoints >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experiencePoints -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        maxHealth += 10;
        maxMana += 5;
        currentHealth = maxHealth;
        currentMana = maxMana;

        Debug.Log($"{name} leveled up to level {level}!");
    }
}
