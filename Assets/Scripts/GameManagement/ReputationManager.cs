using UnityEngine;
using System.Collections.Generic;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager s_instance;

    public Dictionary<Faction, int> reputationValues = new Dictionary<Faction, int>();

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeReputations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeReputations()
    {
        foreach (Faction faction in System.Enum.GetValues(typeof(Faction)))
        {
            if (faction != Faction.None)
            {
                reputationValues[faction] = 0;
            }
        }
    }

    public void AddReputation(Faction faction, int amount)
    {
        if (faction != Faction.None)
        {
            reputationValues[faction] += amount;
            Debug.Log($"Reputation with {faction} changed by {amount}. New value: {reputationValues[faction]}");
        }
    }

    public int GetReputation(Faction faction)
    {
        if (faction != Faction.None)
        {
            return reputationValues[faction];
        }
        return 0;
    }
}
