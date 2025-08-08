using UnityEngine;

public class Workshop : MonoBehaviour
{
    // In a real UI, this would be called when a player clicks an upgrade button.
    public void PurchaseAbilityUpgrade(VerbalAbility ability, AbilityUpgrade upgrade)
    {
        if (ability == null || upgrade == null) return;
        if (PlayerProfile.s_instance.HasUpgrade(ability, upgrade))
        {
            Debug.Log($"Already purchased this upgrade for {ability.name}.");
            return;
        }

        if (CommuneManager.s_instance != null && CommuneManager.s_instance.insightResource >= upgrade.insightCost)
        {
            CommuneManager.s_instance.insightResource -= upgrade.insightCost;
            PlayerProfile.s_instance.PurchaseUpgrade(ability, upgrade);
            Debug.Log($"Successfully purchased upgrade '{upgrade.upgradeName}' for {ability.name}!");
        }
        else
        {
            Debug.Log($"Not enough Insight to purchase this upgrade.");
        }
    }
}
