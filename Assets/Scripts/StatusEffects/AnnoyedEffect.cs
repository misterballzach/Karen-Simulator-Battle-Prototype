using UnityEngine;

[System.Serializable]
public class AnnoyedEffect : StatusEffect
{
    private float damageReduction = 0.5f;

    public AnnoyedEffect()
    {
        name = "Annoyed";
        description = "This unit is annoyed and deals 50% less damage.";
        type = StatusEffectType.Debuff;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
        {
            owner.outgoingDamageModifier -= damageReduction;
        }
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier += damageReduction;
        }
    }
}
