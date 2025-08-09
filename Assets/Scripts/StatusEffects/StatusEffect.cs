using UnityEngine;

public enum StatusEffectType
{
    Buff,
    Debuff
}

[System.Serializable]
public interface IOnDamageTrigger
{
    /// <summary>
    /// Called when the owner is about to take damage.
    /// </summary>
    /// <param name="attacker">The combatant who initiated the damage.</param>
    /// <param name="damage">The initial amount of damage. Can be modified.</param>
    /// <returns>Return false to cancel the damage completely.</returns>
    bool OnTakeDamage(Combatant attacker, ref int damage);
}

public class StatusEffect
{
    public string name;
    [TextArea]
    public string description;
    public int duration; // in turns
    public StatusEffectType type;

    protected Combatant owner;

    public virtual void Apply(Combatant target)
    {
        this.owner = target;
    }

    public virtual void OnTurnStart()
    {
        // Logic to be executed at the start of the turn
    }

    public virtual void OnTurnEnd()
    {
        duration--;
    }

    public virtual void Remove()
    {
        // Logic to be executed when the effect is removed
    }
}
