using UnityEngine;
using System.Collections.Generic;

public class Combatant : MonoBehaviour
{
    [Header("Identity")]
    public Faction faction;
    public KarenClass karenClass;

    [Header("Stats")]
    public int level = 1;
    public int experiencePoints = 0;
    public int xpToNextLevel = 100;
    public int maxEmotionalStamina = 100;
    public int currentEmotionalStamina;
    public int maxCredibility = 100;
    public int currentCredibility;
    public int armor = 0;
    public int passiveAggressiveMeter = 0;
    public int maxPassiveAggressive = 100;

    [Header("Modifiers")]
    public float outgoingDamageModifier = 1f;
    public float incomingDamageModifier = 1f;
    public float incomingHealingModifier = 1f;
    public int credibilityCostModifier = 0;

    public List<VerbalAbility> verbalLoadout = new List<VerbalAbility>();
    public List<VerbalAbility> preparedArguments = new List<VerbalAbility>();
    public List<VerbalAbility> usedArguments = new List<VerbalAbility>();

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public List<RhetoricalClass> rhetoricalWeaknesses = new List<RhetoricalClass>();
    public List<RhetoricalClass> rhetoricalResistances = new List<RhetoricalClass>();

    void Start()
    {
        currentEmotionalStamina = maxEmotionalStamina;
        currentCredibility = maxCredibility;
        ShuffleLoadout();
    }

    public void TakeEmotionalDamage(int amount, RhetoricalClass rhetoricalClass)
    {
        float rhetoricalMultiplier = 1f;
        if (rhetoricalWeaknesses.Contains(rhetoricalClass)) rhetoricalMultiplier = 2f;
        else if (rhetoricalResistances.Contains(rhetoricalClass)) rhetoricalMultiplier = 0.5f;

        int totalDamage = Mathf.RoundToInt(amount * rhetoricalMultiplier * incomingDamageModifier);

        int damageToArmor = Mathf.Min(armor, totalDamage);
        armor -= damageToArmor;
        int damageToHealth = totalDamage - damageToArmor;

        currentEmotionalStamina -= damageToHealth;
        GainPassiveAggression(totalDamage);

        if (currentEmotionalStamina < 0) currentEmotionalStamina = 0;
        Debug.Log($"{name} took {damageToHealth} emotional damage ({damageToArmor} absorbed by armor).");
    }

    public void RecoverStamina(int amount)
    {
        int totalHealing = Mathf.RoundToInt(amount * incomingHealingModifier);
        currentEmotionalStamina += totalHealing;
        if (currentEmotionalStamina > maxEmotionalStamina) currentEmotionalStamina = maxEmotionalStamina;
        Debug.Log($"{name} recovered {totalHealing} stamina.");
    }

    public void PrepareArgument()
    {
        if (verbalLoadout.Count == 0)
        {
            foreach(VerbalAbility ability in usedArguments)
            {
                verbalLoadout.Add(ability);
            }
            usedArguments.Clear();
            ShuffleLoadout();
        }

        if (verbalLoadout.Count > 0)
        {
            VerbalAbility ability = verbalLoadout[0];
            verbalLoadout.RemoveAt(0);
            preparedArguments.Add(ability);
        }
    }

    public bool UseAbility(VerbalAbility ability, Combatant target)
    {
        if (!preparedArguments.Contains(ability)) return false;

        // Check for status effects that prevent ability use
        if (statusEffects.Exists(e => e is ExposedEffect) && (ability.rhetoricalClass == RhetoricalClass.Manipulation || ability.rhetoricalClass == RhetoricalClass.Delusion))
        {
            Debug.Log($"Cannot use {ability.rhetoricalClass} abilities while Exposed!");
            return false;
        }
        if (statusEffects.Exists(e => e is CancelledEffect) && ability.rhetoricalClass == RhetoricalClass.Aggression)
        {
            Debug.Log($"Cannot use {ability.rhetoricalClass} abilities while Cancelled!");
            return false;
        }

        int finalCost = Mathf.Max(0, ability.cost + credibilityCostModifier);

        if (ability is CallTheCopsUltimate)
        {
            if (passiveAggressiveMeter < maxPassiveAggressive) return false;
            passiveAggressiveMeter = 0;
        }
        else if (currentCredibility < finalCost)
        {
            return false;
        }

        if (!(ability is CallTheCopsUltimate))
        {
            currentCredibility -= finalCost;
        }

        ability.Use(this, target);
        preparedArguments.Remove(ability);
        usedArguments.Add(ability);
        return true;
    }

    public void ShuffleLoadout()
    {
        for (int i = 0; i < verbalLoadout.Count; i++)
        {
            VerbalAbility temp = verbalLoadout[i];
            int randomIndex = Random.Range(i, verbalLoadout.Count);
            verbalLoadout[i] = verbalLoadout[randomIndex];
            verbalLoadout[randomIndex] = temp;
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
            if (effect is FlusteredEffect flustered)
            {
                if (flustered.CheckIfTurnSkipped()) return false;
            }
        }
        return true;
    }

    public void OnTurnStart()
    {
        currentCredibility += 10;
        if (currentCredibility > maxCredibility) currentCredibility = maxCredibility;

        foreach (var effect in statusEffects)
        {
            effect.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        if (armor > 0) armor = Mathf.FloorToInt(armor * 0.5f);

        List<StatusEffect> effectsToRemove = new List<StatusEffect>();
        foreach (var effect in statusEffects)
        {
            effect.OnTurnEnd();
            if (effect.duration <= 0) effectsToRemove.Add(effect);
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
        if (experiencePoints >= xpToNextLevel) LevelUp();
    }

    private void LevelUp()
    {
        level++;
        experiencePoints -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        // Base stat increases
        maxEmotionalStamina += 10;
        maxCredibility += 5;

        // Apply class-specific bonuses
        ApplyClassBonuses();

        currentEmotionalStamina = maxEmotionalStamina;
        currentCredibility = maxCredibility;
    }

    private void ApplyClassBonuses()
    {
        // Placeholder for class-specific level up bonuses
        switch (karenClass)
        {
            case KarenClass.ClassicKaren:
                maxPassiveAggressive -= 5; // Gets angry faster
                break;
            case KarenClass.WellnessWitch:
                incomingHealingModifier += 0.1f;
                break;
            case KarenClass.KarenNouveau:
                // Would have logic related to Online Reputation
                break;
            case KarenClass.RepentantKaren:
                // Would have logic related to redemption
                break;
        }
    }

    public void GainPassiveAggression(int amount)
    {
        if (statusEffects.Exists(e => e is CodeBlondeEffect)) return; // Can't gain PA while in Code Blonde

        passiveAggressiveMeter += amount;
        Debug.Log($"{name} gained {amount} PA, now has {passiveAggressiveMeter}.");

        if (passiveAggressiveMeter >= maxPassiveAggressive)
        {
            passiveAggressiveMeter = 0; // Reset meter
            AddStatusEffect(new CodeBlondeEffect());
        }
    }
}
