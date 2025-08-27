using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the display and flow of dialogue conversations.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_instance;

    [Header("UI Elements")]
    [Tooltip("The main panel holding all dialogue UI elements.")]
    public GameObject dialoguePanel;
    [Tooltip("The UI Text element for the speaker's name.")]
    public Text speakerNameText;
    [Tooltip("The UI Text element for the dialogue content.")]
    public Text dialogueText;
    [Tooltip("The UI Image element for the character's sprite.")]
    public Image characterImage;
    [Tooltip("The AudioSource to play dialogue audio clips.")]
    public AudioSource audioSource;
    [Tooltip("The parent transform where choice buttons will be instantiated.")]
    public Transform choicesContainer;
    [Tooltip("The prefab to use for instantiating choice buttons.")]
    public GameObject choiceButtonPrefab;

    private DialogueNode currentNode;
    private List<GameObject> currentChoiceButtons = new List<GameObject>();

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            dialoguePanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts a new dialogue conversation.
    /// </summary>
    /// <param name="startingNode">The first node of the dialogue to display.</param>
    public void StartDialogue(DialogueNode startingNode)
    {
        if (startingNode == null)
        {
            Debug.LogError("StartDialogue was called with a null node.");
            return;
        }
        dialoguePanel.SetActive(true);
        DisplayNode(startingNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        currentNode = node;
        dialogueText.text = node.dialogueText;

        if (node.character != null)
        {
            speakerNameText.text = node.character.characterName;
            if (characterImage != null)
            {
                Sprite sprite = node.character.GetSprite(node.characterExpression);
                if (sprite != null)
                {
                    characterImage.sprite = sprite;
                    characterImage.enabled = true;
                }
                else
                {
                    characterImage.enabled = false;
                }
            }
        }
        else
        {
            speakerNameText.text = "???";
            if (characterImage != null)
            {
                characterImage.enabled = false;
            }
        }

        // Play audio
        if (audioSource != null && node.audioClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(node.audioClip);
        }

        // Trigger events
        node.onNodeStart?.Invoke();

        // Clear previous choice buttons
        foreach (var button in currentChoiceButtons)
        {
            Destroy(button);
        }
        currentChoiceButtons.Clear();

        // Create buttons for choices
        if (node.choices != null && node.choices.Count > 0)
        {
            foreach (Choice choice in node.choices)
            {
                if (CheckRequirements(choice))
                {
                    GameObject choiceButtonObj = Instantiate(choiceButtonPrefab, choicesContainer);
                    choiceButtonObj.GetComponentInChildren<Text>().text = choice.choiceText;
                    choiceButtonObj.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choice));
                    currentChoiceButtons.Add(choiceButtonObj);
                }
            }
        }

        // If no choices were available or valid, check for a linear path
        if (currentChoiceButtons.Count == 0)
        {
            if (node.linearNextNode != null)
            {
                GameObject continueButton = Instantiate(choiceButtonPrefab, choicesContainer);
                continueButton.GetComponentInChildren<Text>().text = "Continue";
                continueButton.GetComponent<Button>().onClick.AddListener(() => DisplayNode(node.linearNextNode));
                currentChoiceButtons.Add(continueButton);
            }
            else
            {
                // If there's no linear path either, create a button to end the dialogue
                GameObject endButton = Instantiate(choiceButtonPrefab, choicesContainer);
                endButton.GetComponentInChildren<Text>().text = "End";
                endButton.GetComponent<Button>().onClick.AddListener(EndDialogue);
                currentChoiceButtons.Add(endButton);
            }
        }
    }

    private bool CheckRequirements(Choice choice)
    {
        if (choice.requirements == null) return true;
        if (ReputationManager.s_instance == null) return true; // No manager, no checks

        foreach (var req in choice.requirements)
        {
            int currentRep = ReputationManager.s_instance.GetReputation(req.faction);
            if (currentRep < req.minValue || currentRep > req.maxValue)
            {
                return false; // Requirement not met
            }
        }
        return true;
    }

    /// <summary>
    /// Processes a player's selected choice, triggers its event, and moves to the next node.
    /// </summary>
    /// <param name="choice">The choice that the player selected.</param>
    public void ChooseOption(Choice choice)
    {
        // Trigger the event associated with the choice
        choice.onChoiceSelected?.Invoke();

        if (choice.nextNode != null)
        {
            DisplayNode(choice.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Ends the current dialogue conversation and hides the dialogue panel.
    /// </summary>
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNode = null;
        Debug.Log("Dialogue ended.");
    }
}
