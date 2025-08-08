using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Mindscape", menuName = "KAREN/Mindscape/Mindscape")]
public class Mindscape : ScriptableObject
{
    public string mindscapeName;
    [TextArea]
    public string description;

    public List<MindscapeEvent> events;
}
