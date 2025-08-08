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
    public int armor = 0;
    public int escalation = 0;
    public int maxEscalation = 100;

    [Header("Modifiers")]
    public float outgoingDamageModifier = 1f;
    public float incomingDamageModifier = 1f;
    public float incomingHealingModifier = 1f;
    public int manaCostModifier = 0;

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
        float elementalMultiplier = 1f;
        if (elementalWeaknesses.Contains(element))
        {
            elementalMultiplier = 2f;
            Debug.Log($"{name} is weak to {element}!");
        }
        else if (elementalResistances.Contains(element))
        {
            elementalMultiplier = 0.5f;
            Debug.Log($"{name} is resistant to {element}!");
        }

        int totalDamage = Mathf.RoundToInt(amount * elementalMultiplier * incomingDamageModifier);

        int damageToArmor = Mathf.Min(armor, totalDamage);
        armor -= damageToArmor;
        int damageToHealth = totalDamage - damageToArmor;

        currentHealth -= damageToHealth;
        GainEscalation(totalDamage); // Gain escalation from total damage before armor

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        Debug.Log($"{name} took {damageToHealth} damage ({damageToArmor} absorbed by armor), now has {currentHealth} health and {armor} armor.");
    }

    public void Heal(int amount)
    {
        int totalHealing = Mathf.RoundToInt(amount * incomingHealingModifier);
        currentHealth += totalHealing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"{name} healed for {totalHealing}, now has {currentHealth} health.");
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
        if (!hand.Contains(card)) return false;

        int finalCost = Mathf.Max(0, card.cost + manaCostModifier);

        // Special check for CallManagerCard
        if (card is CallManagerCard)
        {
            if (escalation < maxEscalation)
            {
                Debug.Log("Not enough escalation to play this card!");
                return false;
            }
            escalation = 0;
        }
        else if (currentMana < finalCost)
        {
            Debug.Log($"Not enough mana to play {card.name}. Needs {finalCost}, has {currentMana}.");
            return false;
        }

        if (!(card is CallManagerCard))
        {
            currentMana -= finalCost;
        }

        card.Use(this, target);
        hand.Remove(card);
        discardPile.Add(card);
        Debug.Log($"{name} played {card.name}.");
        return true;
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

    public bool CanTakeTurn()
    {
        foreach (var effect in statusEffects)
        {
            if (effect is FlusteredEffect flusteredEffect)
            {
                if (flusteredEffect.CheckIfTurnSkipped())
                {
                    return false; // Turn is skipped
                }
            }
        }
        return true;
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
        // Armor decay
        if (armor > 0)
        {
            armor = Mathf.FloorToInt(armor * 0.5f);
            Debug.Log($"{name}'s armor decayed to {armor}.");
        }

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

    public void GainEscalation(int amount)
    {
        escalation += amount;
        if (escalation > maxEscalation)
        {
            escalation = maxEscalation;
        }
        Debug.Log($"{name} gained {amount} escalation, now has {escalation}.");
    }
}
