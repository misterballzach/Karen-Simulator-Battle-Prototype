using UnityEngine;

public interface IAbilityUpgrade
{
    void Apply(VerbalAbility ability);
}

[System.Serializable]
public class AbilityUpgrade : IAbilityUpgrade
{
    public enum UpgradeType { ReduceCost, IncreaseDamage, IncreaseHealing, AddStatusEffect } // Example types

    public string upgradeName;
    public string upgradeDescription;
    public int insightCost;
    public UpgradeType type;
    public int value;
    public StatusEffect statusEffect; // Only used if type is AddStatusEffect

    public void Apply(VerbalAbility ability)
    {
        switch (type)
        {
            case UpgradeType.ReduceCost:
                ability.cost -= value;
                break;
            case UpgradeType.IncreaseDamage:
                ability.damage += value;
                break;
            case UpgradeType.IncreaseHealing:
                ability.healing += value;
                break;
            case UpgradeType.AddStatusEffect:
                // This is also complex. It requires modifying the ability's effects.
                // I will leave this for later.
                Debug.LogWarning("AddStatusEffect upgrade not yet implemented.");
                break;
        }
    }
}
