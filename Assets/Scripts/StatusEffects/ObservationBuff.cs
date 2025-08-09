using UnityEngine;

[System.Serializable]
public class ObservationBuff : StatusEffect
{
    private float critChanceBonus = 0.2f;

    public ObservationBuff()
    {
        name = "Neighborhood Watch";
        description = "This unit is observing closely, increasing their critical hit chance by 20% for 3 turns.";
        type = StatusEffectType.Buff;
        duration = 3;
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        owner.critChance += critChanceBonus;
        Debug.Log($"{owner.name}'s critical hit chance increased by {critChanceBonus * 100}%.");
    }

    public override void Remove()
    {
        base.Remove();
        if (owner != null)
        {
            owner.critChance -= critChanceBonus;
            Debug.Log($"{owner.name}'s critical hit chance returned to normal.");
        }
    }
}
