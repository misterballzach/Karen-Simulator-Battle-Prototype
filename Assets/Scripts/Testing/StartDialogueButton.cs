using UnityEngine;

/// <summary>
/// A component to be placed on a UI Button to start a dialogue.
/// This is used by the test scene setup to create a persistent listener.
/// </summary>
public class StartDialogueButton : MonoBehaviour
{
    [Tooltip("The first dialogue node to display when the button is clicked.")]
    public DialogueNode startingNode;

    /// <summary>
    /// Starts the dialogue using the DialogueManager singleton.
    /// </summary>
    public void StartDialogue()
    {
        if (startingNode != null)
        {
            DialogueManager.s_instance.StartDialogue(startingNode);
            // Optional: disable the button after starting
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Starting node is not set on the StartDialogueButton.");
        }
    }
}
