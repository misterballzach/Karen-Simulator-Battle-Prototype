using UnityEngine;

public abstract class Consequence : ScriptableObject
{
    public abstract void Trigger(Combatant user);
}
