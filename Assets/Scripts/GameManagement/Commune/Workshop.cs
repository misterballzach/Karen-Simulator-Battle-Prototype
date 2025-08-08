using UnityEngine;

public class Workshop : MonoBehaviour
{
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
            // likely by modifying the ScriptableObject asset itself in the editor,
            // or by having a separate system to track upgrades.
            // For this prototype, we'll just set the flag at runtime.
        }
        else
        {
            Debug.Log($"Not enough Insight to upgrade {ability.name}.");
        }
    }
}
