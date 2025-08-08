using UnityEngine;

public enum StatusEffectType
{
    Buff,
    Debuff
}

[System.Serializable]
public class StatusEffect
{
    public string name;
    public string description;
    public int duration; // in turns
    public StatusEffectType type;

    protected Entity owner;

    public virtual void Apply(Entity target)
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
