using UnityEngine;

public enum LocationEffectType { None, ManaCostModification, StartingArmor }

[CreateAssetMenu(fileName = "New Location", menuName = "Locations/Location")]
public class Location : ScriptableObject
{
    public string locationName;
    [TextArea]
    public string description;

    [Header("Location Effect")]
    public LocationEffectType effectType;
    public int effectValue;
}
