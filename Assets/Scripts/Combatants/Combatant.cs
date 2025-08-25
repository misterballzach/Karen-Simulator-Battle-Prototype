using UnityEngine;
using System.Collections.Generic;

public class Combatant : MonoBehaviour
{
    [Header("Identity")]
    public Faction faction;
    public KarenClass karenClass;
    public AIProfile aiProfile;
    public SpriteRenderer characterSprite;

    [Header("Stats")]
    public int level = 1;
    public int experiencePoints = 0;
    public int xpToNextLevel = 100;
    public int maxEmotionalStamina = 100;
    public int currentEmotionalStamina;
    public int maxCredibility = 100;
    public int currentCredibility;
    public int armor = 0;

    [Header("Dual-Axis Meters")]
    public int entitlement = 0;
    public int maxEntitlement = 100;
    public int insight = 0;
    public int maxInsight = 100;

    [Header("Critical Hits")]
    public float critChance = 0.05f; // 5% base chance
    public float critDamageMultiplier = 1.5f; // 150% damage on crit

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

    [HideInInspector]
    public Encounter currentEncounter;

    void Start()
    {
        currentEmotionalStamina = maxEmotionalStamina;
        currentCredibility = maxCredibility;
        ShuffleLoadout();
    }

    public void TakeEmotionalDamage(int amount, RhetoricalClass rhetoricalClass, Combatant attacker)
    {
        // --- Pre-damage Triggers ---
        int modifiedDamage = amount;
        foreach (var effect in statusEffects)
        {
            if (effect is IOnDamageTrigger trigger)
            {
                if (!trigger.OnTakeDamage(attacker, ref modifiedDamage))
                {
                    // If the trigger returns false, the damage is cancelled.
                    Debug.Log($"Damage cancelled by {effect.name}.");
                    return;
                }
            }
        }

        // --- Damage Calculation ---
        bool isCrit = false;
        if (attacker != null && Random.value < attacker.critChance)
        {
            isCrit = true;
            Debug.Log("Critical Hit!");
        }

        float critMultiplier = isCrit ? attacker.critDamageMultiplier : 1f;
        float rhetoricalMultiplier = 1f;
        if (rhetoricalWeaknesses.Contains(rhetoricalClass)) rhetoricalMultiplier = 2f;
        else if (rhetoricalResistances.Contains(rhetoricalClass)) rhetoricalMultiplier = 0.5f;

        int totalDamage = Mathf.RoundToInt(modifiedDamage * critMultiplier * rhetoricalMultiplier * incomingDamageModifier);

        int damageToArmor = Mathf.Min(armor, totalDamage);
        armor -= damageToArmor;
        int damageToHealth = totalDamage - damageToArmor;

        currentEmotionalStamina -= damageToHealth;

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

        // --- Create a temporary, modified instance of the ability ---
        VerbalAbility modifiedAbility = Instantiate(ability); // Creates a runtime copy

        // Apply upgrades from PlayerProfile
        if (PlayerProfile.s_instance != null && PlayerProfile.s_instance.purchasedUpgrades.ContainsKey(ability))
        {
            foreach (var upgrade in PlayerProfile.s_instance.purchasedUpgrades[ability])
            {
                upgrade.Apply(modifiedAbility);
            }
        }

        // --- Check for status effects that prevent ability use ---
        foreach (var effect in statusEffects)
        {
            if (effect.BlockedRhetoric.Contains(modifiedAbility.rhetoricalClass))
            {
                Debug.Log($"Cannot use {modifiedAbility.rhetoricalClass} abilities due to {effect.name}!");
                Destroy(modifiedAbility); // Clean up the temporary instance
                return false;
            }
        }

        // --- Check costs ---
        int finalCost = Mathf.Max(0, modifiedAbility.cost + credibilityCostModifier);

        if (modifiedAbility is CallTheCopsUltimate)
        {
            if (entitlement < maxEntitlement) { Destroy(modifiedAbility); return false; }
            entitlement = 0;
        }
        else if (currentCredibility < finalCost)
        {
            Destroy(modifiedAbility);
            return false;
        }

        // --- Pay costs and execute ---
        if (!(modifiedAbility is CallTheCopsUltimate))
        {
            currentCredibility -= finalCost;
        }

        modifiedAbility.Use(this, target);
        preparedArguments.Remove(ability); // Remove the original card
        usedArguments.Add(ability); // Add the original card to discard

        Destroy(modifiedAbility); // Clean up the temporary instance
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

        maxEmotionalStamina += 10;
        maxCredibility += 5;
        ApplyClassBonuses();
        currentEmotionalStamina = maxEmotionalStamina;
        currentCredibility = maxCredibility;
    }

    private void ApplyClassBonuses()
    {
        switch (karenClass)
        {
            case KarenClass.ClassicKaren:
                maxEntitlement -= 5;
                break;
            case KarenClass.WellnessWitch:
                incomingHealingModifier += 0.1f;
                break;
        }
    }

    public void GainEntitlement(int amount)
    {
        if (statusEffects.Exists(e => e is CodeBlondeEffect)) return;
        entitlement += amount;
        if (entitlement >= maxEntitlement)
        {
            entitlement = 0;
            AddStatusEffect(new CodeBlondeEffect());
        }
    }

    public void GainInsight(int amount)
    {
        insight += amount;
        if (insight >= maxInsight)
        {
            // This is a win condition handled by the Encounter
        }
    }

    public void SetSprite(Sprite newSprite)
    {
        if (characterSprite != null)
        {
            characterSprite.sprite = newSprite;
        }
    }
}
