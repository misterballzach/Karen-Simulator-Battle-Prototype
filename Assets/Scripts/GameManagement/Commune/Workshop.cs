using UnityEngine;

public class Workshop : MonoBehaviour
{
    // In a real UI, this would be called when a player clicks an upgrade button.
    public void PurchaseAbilityUpgrade(VerbalAbility ability, AbilityUpgrade upgrade)
    {
        if (ability == null || upgrade == null)
        {
            Debug.LogWarning("Purchase failed: Ability or upgrade is null.");
            return;
        }

        if (PlayerProfile.s_instance == null)
        {
            Debug.LogError("Purchase failed: PlayerProfile instance is missing.");
            return;
        }

        if (CommuneManager.s_instance == null)
        {
            Debug.LogError("Purchase failed: CommuneManager instance is missing.");
            return;
        }

        if (PlayerProfile.s_instance.HasUpgrade(ability, upgrade))
        {
            Debug.Log($"Already purchased this upgrade for {ability.name}.");
            Debug.Log($"Already purchased this upgrade for {ability.abilityName}.");
            return;
        }

        if (CommuneManager.s_instance.insightResource >= upgrade.insightCost)
        {
            CommuneManager.s_instance.insightResource -= upgrade.insightCost;
            PlayerProfile.s_instance.PurchaseUpgrade(ability, upgrade);

            Debug.Log($"Successfully purchased upgrade '{upgrade.upgradeName}' for {ability.name}! " +
            
            Debug.Log($"Successfully purchased upgrade '{upgrade.upgradeName}' for {ability.abilityName}! " +
                      $"Remaining Insight: {CommuneManager.s_instance.insightResource}");
        }
        else
        {
            Debug.Log($"Not enough Insight to purchase '{upgrade.upgradeName}'. " +
                      $"Required: {upgrade.insightCost}, Available: {CommuneManager.s_instance.insightResource}");
        }
    }
}
