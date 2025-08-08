using UnityEngine;

public abstract class LocationEffect : ScriptableObject
{
    public abstract void Apply(Encounter encounter);
    public abstract void Unapply(Encounter encounter);
}
