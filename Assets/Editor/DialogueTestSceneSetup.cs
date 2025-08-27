using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.Events;

public class DialogueTestSceneSetup
{
    [MenuItem("KAREN/Setup Dialogue Test Scene")]
    public static void SetupScene()
    {
        // --- Create a new Scene ---
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SaveScene(newScene, "Assets/DialogueTestScene.unity");

        // --- Create Test Cube ---
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "TestEventCube";
        var testReceiver = cube.AddComponent<TestEventReceiver>();

        // --- Create Dialogue UI ---
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Dialogue Panel
        var dialoguePanelGO = new GameObject("DialoguePanel", typeof(Image));
        dialoguePanelGO.transform.SetParent(canvas.transform, false);
        var panelRect = dialoguePanelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0.3f);
        panelRect.offsetMin = new Vector2(50, 50);
        panelRect.offsetMax = new Vector2(-50, 50);
        dialoguePanelGO.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
        dialoguePanelGO.SetActive(false);

        // Character Image
        var charImageGO = new GameObject("CharacterImage", typeof(Image));
        charImageGO.transform.SetParent(dialoguePanelGO.transform, false);
        var charImageRect = charImageGO.GetComponent<RectTransform>();
        charImageRect.anchorMin = new Vector2(0, 0.5f);
        charImageRect.anchorMax = new Vector2(0.25f, 0.5f);
        charImageRect.sizeDelta = new Vector2(200, 200);
        charImageRect.anchoredPosition = new Vector2(150, 0);
        var charImage = charImageGO.GetComponent<Image>();
        charImage.preserveAspect = true;

        // Speaker Name Text
        var speakerTextGO = new GameObject("SpeakerName", typeof(Text));
        speakerTextGO.transform.SetParent(dialoguePanelGO.transform, false);
        var speakerTextRect = speakerTextGO.GetComponent<RectTransform>();
        speakerTextRect.anchorMin = new Vector2(0.3f, 0.8f);
        speakerTextRect.anchorMax = new Vector2(0.9f, 0.95f);
        var speakerText = speakerTextGO.GetComponent<Text>();
        speakerText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        speakerText.color = Color.white;
        speakerText.text = "Speaker Name";

        // Dialogue Text
        var dialogueTextGO = new GameObject("DialogueText", typeof(Text));
        dialogueTextGO.transform.SetParent(dialoguePanelGO.transform, false);
        var dialogueTextRect = dialogueTextGO.GetComponent<RectTransform>();
        dialogueTextRect.anchorMin = new Vector2(0.3f, 0.1f);
        dialogueTextRect.anchorMax = new Vector2(0.9f, 0.7f);
        var dialogueText = dialogueTextGO.GetComponent<Text>();
        dialogueText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        dialogueText.color = Color.white;
        dialogueText.text = "This is the dialogue text area.";

        // Choices Container
        var choicesContainerGO = new GameObject("ChoicesContainer");
        choicesContainerGO.transform.SetParent(dialoguePanelGO.transform, false);
        var choicesRect = choicesContainerGO.AddComponent<RectTransform>();
        choicesRect.anchorMin = new Vector2(1, 0);
        choicesRect.anchorMax = new Vector2(1, 1);
        choicesRect.pivot = new Vector2(1, 0.5f);
        choicesRect.sizeDelta = new Vector2(200, 0);
        choicesRect.anchoredPosition = new Vector2(220, 0);
        choicesContainerGO.AddComponent<VerticalLayoutGroup>();

        // --- Create Dialogue Manager ---
        var dmGO = new GameObject("DialogueManager", typeof(DialogueManager), typeof(AudioSource));
        var dialogueManager = dmGO.GetComponent<DialogueManager>();
        dialogueManager.dialoguePanel = dialoguePanelGO;
        dialogueManager.speakerNameText = speakerText;
        dialogueManager.dialogueText = dialogueText;
        dialogueManager.characterImage = charImage;
        dialogueManager.audioSource = dmGO.GetComponent<AudioSource>();
        dialogueManager.choicesContainer = choicesContainerGO.transform;

        // Create a simple prefab for choice buttons
        var choiceButtonPrefab = new GameObject("ChoiceButtonPrefab", typeof(Button), typeof(Image));
        choiceButtonPrefab.GetComponent<Image>().color = new Color(0.2f, 0.3f, 0.8f);
        var choiceTextGO = new GameObject("Text", typeof(Text));
        choiceTextGO.transform.SetParent(choiceButtonPrefab.transform, false);
        choiceTextGO.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        choiceTextGO.GetComponent<Text>().color = Color.white;
        choiceTextGO.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        choiceTextGO.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        choiceTextGO.GetComponent<RectTransform>().anchorMax = Vector2.one;
        choiceTextGO.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        choiceTextGO.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        dialogueManager.choiceButtonPrefab = choiceButtonPrefab;
        choiceButtonPrefab.SetActive(false); // It's a prefab, so it should be inactive

        // --- Create Dialogue Assets ---
        string path = "Assets/Resources/TestDialogue/";
        if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/TestDialogue")) AssetDatabase.CreateFolder("Assets/Resources", "TestDialogue");

        // Character
        var character = ScriptableObject.CreateInstance<Character>();
        character.characterName = "Cassandra";
        character.expressions = new System.Collections.Generic.List<CharacterExpression>
        {
            new CharacterExpression { name = "neutral", sprite = Resources.Load<Sprite>("Characters/PNG/Default/face_a") },
            new CharacterExpression { name = "angry", sprite = Resources.Load<Sprite>("Characters/PNG/Default/face_b") }
        };
        AssetDatabase.CreateAsset(character, path + "Cassandra.asset");

        // Audio
        var audioClip = Resources.Load<AudioClip>("Audio/UI/click1");

        // Nodes
        var node4 = CreateNode(path, "Node4", character, "angry", "You clicked the button! The cube changed color. This is the end of the dialogue.", audioClip, null, null);
        var node3 = CreateNode(path, "Node3", character, "neutral", "This is a linear node. There are no choices here. Click continue to see what happens.", audioClip, node4, null);
        var node2 = CreateNode(path, "Node2", character, "angry", "Don't press my button!", audioClip, null, new System.Collections.Generic.List<ChoiceData> {
            new ChoiceData { text = "Press the button", nextNode = node4, eventTarget = testReceiver.gameObject, eventMethodName = "ChangeColor" }
        });
        var node1 = CreateNode(path, "Node1", character, "neutral", "Hello! This is a test of the new dialogue system. You can have choices, or just linear text.", audioClip, null, new System.Collections.Generic.List<ChoiceData> {
            new ChoiceData { text = "Tell me more (linear path)", nextNode = node3 },
            new ChoiceData { text = "Show me a choice with an event", nextNode = node2 }
        });

        // --- Create Start Button ---
        var startButtonGO = new GameObject("StartDialogueButton", typeof(Button), typeof(Image));
        startButtonGO.transform.SetParent(canvas.transform, false);
        var startButtonRect = startButtonGO.GetComponent<RectTransform>();
        startButtonRect.sizeDelta = new Vector2(160, 30);
        startButtonRect.anchoredPosition = new Vector2(0, 200);
        var startButton = startButtonGO.GetComponent<Button>();
        var startButtonText = new GameObject("Text", typeof(Text)).GetComponent<Text>();
        startButtonText.transform.SetParent(startButtonGO.transform, false);
        startButtonText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        startButtonText.text = "Start Dialogue";
        startButtonText.color = Color.black;
        startButtonText.alignment = TextAnchor.MiddleCenter;

        UnityAction action = () => { dialogueManager.StartDialogue(node1); };
        startButton.onClick.AddListener(action);

        EditorUtility.SetDirty(startButton); // This is needed to save the listener

        // --- Finalize ---
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.MarkSceneDirty(newScene);
        Debug.Log("Dialogue Test Scene setup complete!");
    }

    private static DialogueNode CreateNode(string path, string assetName, Character character, string expression, string text, AudioClip clip, DialogueNode linearNext, System.Collections.Generic.List<ChoiceData> choicesData)
    {
        var node = ScriptableObject.CreateInstance<DialogueNode>();
        node.character = character;
        node.characterExpression = expression;
        node.dialogueText = text;
        node.audioClip = clip;
        node.linearNextNode = linearNext;

        if (choicesData != null)
        {
            node.choices = new System.Collections.Generic.List<Choice>();
            foreach (var choiceData in choicesData)
            {
                var choice = new Choice { choiceText = choiceData.text, nextNode = choiceData.nextNode, onChoiceSelected = new UnityEvent() };
                if (choiceData.eventTarget != null)
                {
                    UnityAction action = () => { choiceData.eventTarget.SendMessage(choiceData.eventMethodName); };
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(choice.onChoiceSelected, action);
                }
                node.choices.Add(choice);
            }
        }

        AssetDatabase.CreateAsset(node, path + assetName + ".asset");
        return node;
    }
}

public class ChoiceData
{
    public string text;
    public DialogueNode nextNode;
    public GameObject eventTarget;
    public string eventMethodName;
}
