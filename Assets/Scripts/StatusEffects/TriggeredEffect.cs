using UnityEngine;

[System.Serializable]
public class TriggeredEffect : StatusEffect
{
    private float modifier = 0.5f; // 50% change

    public TriggeredEffect()
    {
        name = "Triggered";
        description = "This unit is triggered. They deal 50% more damage but also take 50% more damage.";
        type = StatusEffectType.Debuff; // It's a double-edged sword
        duration = 2;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
        {
            owner.outgoingDamageModifier += modifier;
            owner.incomingDamageModifier += modifier;
        }
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier -= modifier;
            owner.incomingDamageModifier -= modifier;
        }
    }
}
