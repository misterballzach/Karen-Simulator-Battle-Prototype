[System.Serializable]
public class AbilityUpgrade
{
    public enum UpgradeType { ReduceCost, IncreaseDamage, AddStatusEffect } // Example types

    public string upgradeName;
    public string upgradeDescription;
    public int insightCost;
    public UpgradeType type;
    public int value;
    public StatusEffect statusEffect; // Only used if type is AddStatusEffect
}
