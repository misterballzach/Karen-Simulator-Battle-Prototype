using UnityEngine;
using System.Collections.Generic;

public class CommuneManager : MonoBehaviour
{
    public static CommuneManager s_instance;

    [Header("Resources")]
    public int communityTrust = 0;
    public int insightResource = 0;

    [Header("Members")]
    public List<LiberatedKaren> members = new List<LiberatedKaren>();

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

    public void AddNewMember(Combatant liberatedCombatant)
    {
        LiberatedKaren newMember = new LiberatedKaren(liberatedCombatant.name, liberatedCombatant.karenClass);
        members.Add(newMember);
        Debug.Log($"{liberatedCombatant.name} has joined the commune!");
    }
}
