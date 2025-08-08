using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // PlayerProfile Data
    public List<VerbalAbility> masterAbilityCollection;
    public List<VerbalAbility> currentDeck;
    // Dictionary for purchasedUpgrades needs to be handled via lists for serialization
    public List<VerbalAbility> upgradedAbilityKeys;
    public List<AbilityUpgradeListWrapper> upgradedAbilityValues;

    // ReputationManager Data
    public List<Faction> reputationFactionKeys;
    public List<int> reputationFactionValues;

    // CommuneManager Data
    public int communityTrust;
    public int insightResource;
    public List<LiberatedKaren> members;

    // This wrapper is needed because Unity can't serialize a list of lists directly.
    [System.Serializable]
    public struct AbilityUpgradeListWrapper
    {
        public List<AbilityUpgrade> upgrades;
    }
}
