using UnityEngine;

public class SelfBuffStatusEffect : StatusEffect
{
    public SelfBuffStatusEffect()
    {
        this.name = "Essential Oil Vigor";
        this.description = "Outgoing damage increased by 30% for 2 turns.";
        this.duration = 2;
        this.type = StatusEffectType.Buff;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
            owner.outgoingDamageModifier += 0.3f;
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
            owner.outgoingDamageModifier -= 0.3f;
    }
}
