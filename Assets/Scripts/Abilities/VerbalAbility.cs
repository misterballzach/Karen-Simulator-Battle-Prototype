using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Verbal Ability", menuName = "KAREN/Verbal Ability")]
public abstract class VerbalAbility : ScriptableObject
{
    [Header("Display Info")]
    public new string name;
    [TextArea]
    public string description;
    public Sprite artwork;

    [Header("Gameplay")]
    public int cost; // Credibility cost
    public RhetoricalClass rhetoricalClass;
    public bool isDebuff; // Used for passive triggers like Petty Solidarity
    public int cooldown;
    [Range(0, 1)]
    public float escalationRisk;

    [Header("Reputation")]
    public List<ReputationModifier> reputationModifiers;

    [Header("Upgrades")]
    public List<AbilityUpgrade> availableUpgrades;

    public abstract void Use(Combatant user, Combatant target);
}
