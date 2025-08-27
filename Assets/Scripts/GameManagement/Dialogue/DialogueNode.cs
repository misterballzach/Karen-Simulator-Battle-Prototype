using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent onChoiceSelected;
    public List<ReputationRequirement> requirements;
}

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "KAREN/Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public Character character;
    public string characterExpression; // e.g., "happy", "angry"

    [TextArea]
    public string dialogueText;

    [Header("Events")]
    public UnityEvent onNodeStart;

    [Header("Audio")]
    public AudioClip audioClip;

    [Header("Choices")]
    public List<Choice> choices;

    [Header("Linear Progression")]
    public DialogueNode linearNextNode;
}
