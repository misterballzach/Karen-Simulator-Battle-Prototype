using UnityEngine;

[System.Serializable]
public class CodeBlondeEffect : StatusEffect
{
    private float damageBuff = 0.5f;

    public CodeBlondeEffect()
    {
        name = "Code Blonde";
        description = "A state of pure, unadulterated fury. All cooldowns are reset and damage is increased by 50%.";
        type = StatusEffectType.Buff;
        duration = 3;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        if (owner != null)
        {
            owner.outgoingDamageModifier += damageBuff;

            Encounter encounter = Object.FindFirstObjectByType<Encounter>();
            if (encounter != null)
            {
                encounter.ClearCooldowns(owner);
            }
            Debug.Log($"{owner.name} has entered a state of CODE BLONDE!");
        }
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.outgoingDamageModifier -= damageBuff;
            Debug.Log($"{owner.name} has calmed down.");
        }
    }
}
