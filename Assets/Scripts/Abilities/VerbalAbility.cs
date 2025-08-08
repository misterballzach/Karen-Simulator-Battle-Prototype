using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Verbal Ability", menuName = "KAREN/Verbal Ability")]
public class VerbalAbility : ScriptableObject
{
    [Header("Display Info")]
    public new string name;
    [TextArea]
    public string description;
    public Sprite artwork;

    [Header("Gameplay")]
    public int cost; // Credibility cost
    public RhetoricalClass rhetoricalClass;
    public int cooldown;
    [Range(0, 1)]
    public float escalationRisk;

    [Header("Reputation")]
    public List<ReputationModifier> reputationModifiers;
    [Header("Upgrades")]
    public bool isUpgraded = false;
    public int upgradeCost = 50; // Insight cost
    [TextArea]
    public string upgradedDescription;
}
