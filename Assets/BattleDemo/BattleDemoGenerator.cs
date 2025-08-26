using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BattleDemoGenerator : MonoBehaviour
{
    [Header("Assets (Drag from Project)")]
    public Sprite combatantSprite;
    public Font uiFont; // Optional: for better text rendering

    private Encounter encounter;

    void Start()
    {
        // --- Create Core Systems ---
        CreateEventSystem();
        Canvas canvas = CreateCanvas();
        GameObject encounterGO = new GameObject("EncounterManager");
        encounter = encounterGO.AddComponent<Encounter>();

        // --- Create Abilities and AI ---
        QuickRetortAbility quickRetort = ScriptableObject.CreateInstance<QuickRetortAbility>();
        quickRetort.name = "Quick Retort";
        quickRetort.description = "A swift, cutting remark.";
        quickRetort.cost = 10;
        quickRetort.damage = 20;
        quickRetort.rhetoricalClass = RhetoricalClass.Aggression;

        DefensiveStanceAbility defensiveStance = ScriptableObject.CreateInstance<DefensiveStanceAbility>();
        defensiveStance.name = "Defensive Stance";
        defensiveStance.description = "Brace for impact.";
        defensiveStance.cost = 10;
        defensiveStance.armorGain = 15;
        defensiveStance.rhetoricalClass = RhetoricalClass.Vulnerability;

        DemoEnemyAI enemyAI = ScriptableObject.CreateInstance<DemoEnemyAI>();

        // --- Create Combatants ---
        Combatant player = CreateCombatant("Player", new Vector3(-3, 0, 0), Faction.Player);
        player.verbalLoadout = new List<VerbalAbility> { quickRetort, defensiveStance, quickRetort, defensiveStance, quickRetort };

        Combatant enemy = CreateCombatant("Enemy", new Vector3(3, 0, 0), Faction.Enemy);
        enemy.aiProfile = enemyAI;
        enemy.verbalLoadout = new List<VerbalAbility> { quickRetort, defensiveStance, quickRetort };

        // --- Link everything to the Encounter ---
        encounter.playerParty = new List<Combatant> { player };
        encounter.enemyParty = new List<Combatant> { enemy };
        player.currentEncounter = encounter;
        enemy.currentEncounter = encounter;

        // --- Create UI ---
        ArgumentHandUI handUI = CreateArgumentHandUI(canvas, player, encounter);
        encounter.playerHandUI = handUI;
        CreateStatusUI(canvas, player, enemy);
        CreateEndTurnButton(canvas, encounter);

        Debug.Log("Battle Demo Generated. Starting encounter...");
    }

    // --- Helper Methods for Creation ---

    private Combatant CreateCombatant(string name, Vector3 position, Faction faction)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;

        if (combatantSprite != null)
        {
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = combatantSprite;
            sr.transform.localScale = new Vector3(3, 3, 3);
        }

        Combatant combatant = go.AddComponent<Combatant>();
        combatant.faction = faction;
        combatant.maxEmotionalStamina = 100;
        combatant.maxCredibility = 100;
        return combatant;
    }

    private Canvas CreateCanvas()
    {
        GameObject canvasGO = new GameObject("BattleCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private void CreateEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }

    private ArgumentHandUI CreateArgumentHandUI(Canvas canvas, Combatant player, Encounter currentEncounter)
    {
        // Hand Container
        GameObject handContainerGO = new GameObject("HandContainer");
        handContainerGO.transform.SetParent(canvas.transform, false);
        RectTransform handRect = handContainerGO.AddComponent<RectTransform>();
        handRect.anchorMin = new Vector2(0.5f, 0);
        handRect.anchorMax = new Vector2(0.5f, 0);
        handRect.pivot = new Vector2(0.5f, 0);
        handRect.anchoredPosition = new Vector2(0, 20);
        handRect.sizeDelta = new Vector2(800, 150);

        HorizontalLayoutGroup layout = handContainerGO.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        ArgumentHandUI handUI = handContainerGO.AddComponent<ArgumentHandUI>();
        handUI.player = player;

        // Create the card "prefab" template
        GameObject cardTemplate = new GameObject("CardTemplate");
        cardTemplate.transform.SetParent(handContainerGO.transform);
        cardTemplate.AddComponent<Image>();
        RectTransform cardRect = cardTemplate.GetComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(120, 150);

        // Add UI elements to the card template
        Text nameText = CreateText(cardTemplate, "NameText", "Ability Name", 14, new Vector2(0, 60));
        Text descText = CreateText(cardTemplate, "DescriptionText", "Description", 10, new Vector2(0, 0));
        Text costText = CreateText(cardTemplate, "CostText", "Cost: X", 12, new Vector2(0, -60));

        // Add the controller script and set up its references
        AbilityCardUI cardUI = cardTemplate.AddComponent<AbilityCardUI>();
        cardUI.nameText = nameText;
        cardUI.descriptionText = descText;
        cardUI.costText = costText;

        handUI.abilityPrefab = cardTemplate;
        cardTemplate.SetActive(false); // Deactivate the template

        // Override the UpdateHandUI to use our custom setup
        handUI.gameObject.AddComponent<CustomHandUIUpdater>().Setup(handUI, player, currentEncounter, cardTemplate);

        return handUI;
    }

    private void CreateStatusUI(Canvas canvas, Combatant player, Combatant enemy)
    {
        // Player Status
        GameObject playerStatusGO = new GameObject("PlayerStatus");
        playerStatusGO.transform.SetParent(canvas.transform, false);
        RectTransform playerRect = playerStatusGO.AddComponent<RectTransform>();
        playerRect.anchorMin = new Vector2(0, 1);
        playerRect.anchorMax = new Vector2(0, 1);
        playerRect.pivot = new Vector2(0, 1);
        playerRect.anchoredPosition = new Vector2(20, -20);
        playerStatusGO.AddComponent<CombatantStatusUI>().Initialize(player, "Player");

        // Enemy Status
        GameObject enemyStatusGO = new GameObject("EnemyStatus");
        enemyStatusGO.transform.SetParent(canvas.transform, false);
        RectTransform enemyRect = enemyStatusGO.AddComponent<RectTransform>();
        enemyRect.anchorMin = new Vector2(1, 1);
        enemyRect.anchorMax = new Vector2(1, 1);
        enemyRect.pivot = new Vector2(1, 1);
        enemyRect.anchoredPosition = new Vector2(-20, -20);
        enemyStatusGO.AddComponent<CombatantStatusUI>().Initialize(enemy, "Enemy");
    }

    private void CreateEndTurnButton(Canvas canvas, Encounter currentEncounter)
    {
        GameObject buttonGO = new GameObject("EndTurnButton");
        buttonGO.transform.SetParent(canvas.transform, false);
        buttonGO.AddComponent<Image>();
        Button button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(currentEncounter.OnEndTurnButton);

        RectTransform rect = buttonGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = new Vector2(-20, 20);
        rect.sizeDelta = new Vector2(160, 50);

        CreateText(buttonGO, "ButtonText", "End Turn", 20, Vector2.zero);
    }

    private Text CreateText(GameObject parent, string name, string content, int fontSize, Vector2 position)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(parent.transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = content;
        text.font = uiFont != null ? uiFont : Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;
        textGO.GetComponent<RectTransform>().anchoredPosition = position;
        textGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        return text;
    }
}
