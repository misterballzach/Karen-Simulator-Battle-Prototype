using UnityEngine;

[System.Serializable]
public class ConfidenceLossDebuff : StatusEffect
{
    private float modifierAmount = 0.25f; // A 25% reduction

    public ConfidenceLossDebuff()
    {
        name = "Confidence Lost";
        description = "This unit's confidence is shaken, reducing their damage by 25% for 2 turns.";
        type = StatusEffectType.Debuff;
        duration = 2;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        owner.outgoingDamageModifier -= modifierAmount;
        Debug.Log($"{owner.name}'s outgoing damage modifier decreased by {modifierAmount}.");
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier += modifierAmount;
            Debug.Log($"{owner.name}'s outgoing damage modifier returned to normal.");
        }
    }
}
