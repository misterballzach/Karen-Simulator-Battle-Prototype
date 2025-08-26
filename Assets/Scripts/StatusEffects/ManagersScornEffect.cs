using UnityEngine;

[System.Serializable]
public class ManagersScornEffect : StatusEffect
{
    private float damageReduction = 0.25f;
    private float healingReduction = 0.5f;

    public ManagersScornEffect()
    {
        name = "Manager's Scorn";
        description = "This unit is being scorned by the manager. Deals 25% less damage and receives 50% less healing.";
        type = StatusEffectType.Debuff;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
        {
            owner.outgoingDamageModifier -= damageReduction;
            owner.incomingHealingModifier -= healingReduction;
        }
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier += damageReduction;
            owner.incomingHealingModifier += healingReduction;
        }
    }
}
