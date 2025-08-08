using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ReputationRequirement
{
    public Faction faction;
    public int minValue;
    public int maxValue;
}

[System.Serializable]
public struct Choice
{
    [TextArea]
    public string choiceText;
    public DialogueNode nextNode;
    public List<ReputationRequirement> requirements;
}

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "KAREN/Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string speakerName;
    [TextArea]
    public string dialogueText;

    [Header("Choices")]
    public List<Choice> choices;

    [Header("Linear Progression")]
    public DialogueNode linearNextNode;
}
