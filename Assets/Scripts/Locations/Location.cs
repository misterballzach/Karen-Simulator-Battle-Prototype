using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Location", menuName = "KAREN/Location")]
public class Location : ScriptableObject
{
    public string locationName;
    [TextArea]
    public string description;

    public List<LocationEffect> locationEffects;
}
