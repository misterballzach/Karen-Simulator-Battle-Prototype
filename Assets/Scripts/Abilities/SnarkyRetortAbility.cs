using UnityEngine;

[CreateAssetMenu(fileName = "Snarky Retort", menuName = "KAREN/Abilities/Snarky Retort")]
public class SnarkyRetortAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        // This ability applies a buff to the user.
        Debug.Log($"{user.name} prepares a Snarky Retort.");
        user.AddStatusEffect(new SnarkyRetortEffect());
    }
}
