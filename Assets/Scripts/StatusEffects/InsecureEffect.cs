using UnityEngine;

[System.Serializable]
public class InsecureEffect : StatusEffect
{
    private float modifierAmount = 0.3f; // 30% more damage taken

    public InsecureEffect()
    {
        name = "Insecure";
        description = "This unit has been made insecure, increasing their incoming damage by 30% for 2 turns.";
        type = StatusEffectType.Debuff;
        duration = 2;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        owner.incomingDamageModifier += modifierAmount;
        Debug.Log($"{owner.name} has been made insecure, increasing their incoming damage.");
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.incomingDamageModifier -= modifierAmount;
            Debug.Log($"{owner.name} is no longer insecure, their incoming damage returns to normal.");
        }
    }
}
