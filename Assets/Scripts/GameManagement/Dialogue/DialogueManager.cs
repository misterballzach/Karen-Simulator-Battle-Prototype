using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    public Transform choicesContainer;
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

    public void StartDialogue(DialogueNode startingNode)
    {
        if (startingNode == null) return;
        dialoguePanel.SetActive(true);
        DisplayNode(startingNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        currentNode = node;
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;

        // Clear old choices
        foreach (var button in currentChoiceButtons)
        {
            Destroy(button);
        }
        currentChoiceButtons.Clear();

        // Create new choices if they exist
        if (node.choices != null && node.choices.Count > 0)
        {
            foreach (Choice choice in node.choices)
            {
                GameObject choiceButtonObj = Instantiate(choiceButtonPrefab, choicesContainer);
                choiceButtonObj.GetComponentInChildren<Text>().text = choice.choiceText;
                choiceButtonObj.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choice));
                currentChoiceButtons.Add(choiceButtonObj);
            }
        }
        // If no choices, the dialogue might end or continue linearly after a click
    }

    public void ChooseOption(Choice choice)
    {
        if (choice.nextNode != null)
        {
            DisplayNode(choice.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNode = null;
        Debug.Log("Dialogue ended.");
        // Potentially notify other systems that dialogue is over
    }
}
