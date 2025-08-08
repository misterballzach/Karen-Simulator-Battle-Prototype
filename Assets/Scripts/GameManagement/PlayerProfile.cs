using UnityEngine;
using System.Collections.Generic;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile s_instance;

    public List<VerbalAbility> masterAbilityCollection = new List<VerbalAbility>();
    public List<VerbalAbility> currentDeck = new List<VerbalAbility>();

    // This is tricky to serialize. A custom solution would be needed for a real game.
    // For the prototype, this will only work at runtime.
    public Dictionary<VerbalAbility, List<AbilityUpgrade>> purchasedUpgrades = new Dictionary<VerbalAbility, List<AbilityUpgrade>>();

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasUpgrade(VerbalAbility ability, AbilityUpgrade upgrade)
    {
        return purchasedUpgrades.ContainsKey(ability) && purchasedUpgrades[ability].Contains(upgrade);
    }

    public void PurchaseUpgrade(VerbalAbility ability, AbilityUpgrade upgrade)
    {
        if (!purchasedUpgrades.ContainsKey(ability))
        {
            purchasedUpgrades[ability] = new List<AbilityUpgrade>();
        }
        purchasedUpgrades[ability].Add(upgrade);
    }
}
