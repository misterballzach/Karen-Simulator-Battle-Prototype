using UnityEngine;

[System.Serializable]
public class FiredEffect : StatusEffect
{
    private float originalDamageModifier;

    public FiredEffect()
    {
        name = "Fired!";
        description = "This unit has been fired by the manager. They are stunned and cannot deal damage.";
        type = StatusEffectType.Debuff;
        duration = 2; // Lasts 2 turns
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
        {
            originalDamageModifier = owner.outgoingDamageModifier;
            owner.outgoingDamageModifier = 0f;
        }
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier = originalDamageModifier;
        }
    }
}
