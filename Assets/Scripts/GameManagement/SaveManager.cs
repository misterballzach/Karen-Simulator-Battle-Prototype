using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager s_instance;

    private string saveFilePath;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "karen_save.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        Debug.Log("Saving game...");
        SaveData data = new SaveData();

        // 1. Populate data from PlayerProfile
        data.masterAbilityCollection = PlayerProfile.s_instance.masterAbilityCollection;
        data.currentDeck = PlayerProfile.s_instance.currentDeck;

        data.upgradedAbilityKeys = new List<VerbalAbility>();
        data.upgradedAbilityValues = new List<SaveData.AbilityUpgradeListWrapper>();
        foreach(var kvp in PlayerProfile.s_instance.purchasedUpgrades)
        {
            data.upgradedAbilityKeys.Add(kvp.Key);
            data.upgradedAbilityValues.Add(new SaveData.AbilityUpgradeListWrapper { upgrades = kvp.Value });
        }

        // 2. Populate data from ReputationManager
        data.reputationFactionKeys = new List<Faction>();
        data.reputationFactionValues = new List<int>();
        foreach(var kvp in ReputationManager.s_instance.reputationValues)
        {
            data.reputationFactionKeys.Add(kvp.Key);
            data.reputationFactionValues.Add(kvp.Value);
        }

        // 3. Populate data from CommuneManager
        data.communityTrust = CommuneManager.s_instance.communityTrust;
        data.insightResource = CommuneManager.s_instance.insightResource;
        data.members = CommuneManager.s_instance.members;

        // --- Save to JSON ---
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        Debug.Log("Loading game...");
        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // --- Apply loaded data ---
        // 1. Apply to PlayerProfile
        PlayerProfile.s_instance.masterAbilityCollection = data.masterAbilityCollection;
        PlayerProfile.s_instance.currentDeck = data.currentDeck;
        PlayerProfile.s_instance.purchasedUpgrades = new Dictionary<VerbalAbility, List<AbilityUpgrade>>();
        for(int i = 0; i < data.upgradedAbilityKeys.Count; i++)
        {
            PlayerProfile.s_instance.purchasedUpgrades[data.upgradedAbilityKeys[i]] = data.upgradedAbilityValues[i].upgrades;
        }

        // 2. Apply to ReputationManager
        ReputationManager.s_instance.reputationValues = new Dictionary<Faction, int>();
        for(int i = 0; i < data.reputationFactionKeys.Count; i++)
        {
            ReputationManager.s_instance.reputationValues[data.reputationFactionKeys[i]] = data.reputationFactionValues[i];
        }

        // 3. Apply to CommuneManager
        CommuneManager.s_instance.communityTrust = data.communityTrust;
        CommuneManager.s_instance.insightResource = data.insightResource;
        CommuneManager.s_instance.members = data.members;

        Debug.Log("Game loaded successfully.");
    }
}
