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

/// <summary>
/// Represents a single choice a player can make in a dialogue.
/// </summary>
[System.Serializable]
public struct Choice
{
    [Tooltip("The text to display for this choice.")]
    [TextArea]
    public string choiceText;
    [Tooltip("The next dialogue node to display if this choice is selected.")]
    public DialogueNode nextNode;
    [Tooltip("An event that is triggered when this choice is selected.")]
    public UnityEvent onChoiceSelected;
    [Tooltip("A list of reputation requirements needed for this choice to be visible.")]
    public List<ReputationRequirement> requirements;
}

/// <summary>
/// A ScriptableObject representing a single node or piece of dialogue in a conversation.
/// </summary>
[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "KAREN/Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Tooltip("The character who is speaking.")]
    public Character character;
    [Tooltip("The expression the character should have. This corresponds to the 'name' in the Character's expression list.")]
    public string characterExpression;

    [Tooltip("The dialogue text to be displayed.")]
    [TextArea]
    public string dialogueText;

    [Header("Events")]
    [Tooltip("Event triggered when this dialogue node is displayed.")]
    public UnityEvent onNodeStart;

    [Header("Audio")]
    [Tooltip("An audio clip to play when this node is displayed.")]
    public AudioClip audioClip;

    [Header("Choices")]
    [Tooltip("A list of choices the player can make from this node. If empty, the dialogue will proceed to the Linear Next Node.")]
    public List<Choice> choices;

    [Header("Linear Progression")]
    [Tooltip("The next node to display if there are no available choices. If this is also null, the dialogue will end.")]
    public DialogueNode linearNextNode;
}
