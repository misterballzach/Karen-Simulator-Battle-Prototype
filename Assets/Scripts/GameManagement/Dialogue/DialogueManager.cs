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

        foreach (var button in currentChoiceButtons)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(button);
        }
        currentChoiceButtons.Clear();

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
    }
}
