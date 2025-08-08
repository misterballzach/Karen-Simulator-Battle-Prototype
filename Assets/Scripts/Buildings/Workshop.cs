using UnityEngine;

public class Workshop : Building
{
    private void Awake()
    {
        buildingName = "Workshop";
        cost = 100; // Example cost
    }

    public void UpgradeAbility(VerbalAbility ability)
    {
        if (ability == null) return;
        if (ability.isUpgraded)
        {
            Debug.Log($"{ability.name} is already upgraded.");
            return;
        }

        if (CommuneManager.s_instance != null && CommuneManager.s_instance.insightResource >= ability.upgradeCost)
        {
            CommuneManager.s_instance.insightResource -= ability.upgradeCost;
            ability.isUpgraded = true;
            Debug.Log($"Successfully upgraded {ability.name}!");
            // A real implementation would need to save this change permanently,
            // or by having a separate system to track upgrades.
            // For this prototype, we'll just set the flag at runtime.
        }
        else
        {
            Debug.Log($"Not enough Insight to upgrade {ability.name}.");
        }
    }
}
