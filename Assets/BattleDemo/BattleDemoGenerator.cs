using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BattleDemoGenerator : MonoBehaviour
{
    [Header("Assets (Drag from Project)")]
    public Sprite combatantSprite;
    public Font uiFont;

    private DemoArgumentHandUI handUI;
    private Combatant player;

    void Start()
    {
        // --- Create Core Systems ---
        CreateEventSystem();
        Canvas canvas = CreateCanvas();
        Encounter encounter = CreateEncounter();

        // --- Create Abilities and AI ---
        QuickRetortAbility quickRetort = ScriptableObject.CreateInstance<QuickRetortAbility>();
        quickRetort.name = "Quick Retort";
        quickRetort.description = "A swift, cutting remark that deals 20 damage.";
        quickRetort.cost = 10;
        quickRetort.damage = 20;
        quickRetort.rhetoricalClass = RhetoricalClass.Aggression;

        DefensiveStanceAbility defensiveStance = ScriptableObject.CreateInstance<DefensiveStanceAbility>();
        defensiveStance.name = "Defensive Stance";
        defensiveStance.description = "Brace for impact, gaining 15 armor.";
        defensiveStance.cost = 10;
        defensiveStance.armorGain = 15;
        defensiveStance.rhetoricalClass = RhetoricalClass.Vulnerability;

        DemoEnemyAI enemyAI = ScriptableObject.CreateInstance<DemoEnemyAI>();

        // --- Create and Configure Combatants ---
        this.player = CreateCombatant("Player", new Vector3(-3, 0, 0), Faction.Player);
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
        this.handUI = CreateArgumentHandUI(canvas, player, encounter);
        CreateStatusUI(canvas, player, enemy);
        CreateEndTurnButton(canvas, encounter);

        // --- Final Initialization Step ---
        player.Initialize();
        enemy.Initialize();

        // Manually start the encounter now that everything is linked
        encounter.BeginEncounter();

        Debug.Log("Battle Demo Generated. Starting encounter...");
    }

    // This is a simple polling mechanism for the demo. A more advanced implementation
    // would use events to trigger UI updates, but this is clear and effective for the demo.
    void Update()
    {
        if (handUI != null && player != null)
        {
            // If the number of card objects in the UI doesn't match the number
            // of prepared arguments in the player's hand, redraw the hand.
            if (handUI.transform.childCount != player.preparedArguments.Count)
            {
                handUI.UpdateHand();
            }
        }
    }

    // --- Helper Methods for Creation ---

    private Encounter CreateEncounter()
    {
        GameObject encounterGO = new GameObject("EncounterManager");
        Encounter encounter = encounterGO.AddComponent<Encounter>();
        encounter.autoStart = false; // We will start it manually
        return encounter;
    }

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

    private DemoArgumentHandUI CreateArgumentHandUI(Canvas canvas, Combatant player, Encounter encounter)
    {
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

        DemoArgumentHandUI handUI = handContainerGO.AddComponent<DemoArgumentHandUI>();

        GameObject cardTemplate = new GameObject("CardTemplate", typeof(RectTransform));
        cardTemplate.transform.SetParent(handContainerGO.transform);
        cardTemplate.AddComponent<Image>();
        cardTemplate.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 150);

        Text nameText = CreateText(cardTemplate, "NameText", "Ability Name", 14, new Vector2(0, 60));
        Text descText = CreateText(cardTemplate, "DescriptionText", "Description", 10, new Vector2(0, 0));
        Text costText = CreateText(cardTemplate, "CostText", "Cost: X", 12, new Vector2(0, -60));

        AbilityCardUI cardUI = cardTemplate.AddComponent<AbilityCardUI>();
        cardUI.nameText = nameText;
        cardUI.descriptionText = descText;
        cardUI.costText = costText;

        handUI.cardPrefabTemplate = cardTemplate;
        handUI.Setup(player, encounter);
        cardTemplate.SetActive(false);

        return handUI;
    }

    private void CreateStatusUI(Canvas canvas, Combatant player, Combatant enemy)
    {
        GameObject playerStatusGO = new GameObject("PlayerStatus", typeof(RectTransform));
        playerStatusGO.transform.SetParent(canvas.transform, false);
        playerStatusGO.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        playerStatusGO.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        playerStatusGO.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        playerStatusGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -20);
        playerStatusGO.AddComponent<CombatantStatusUI>().Initialize(player, "Player", uiFont);

        GameObject enemyStatusGO = new GameObject("EnemyStatus", typeof(RectTransform));
        enemyStatusGO.transform.SetParent(canvas.transform, false);
        enemyStatusGO.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        enemyStatusGO.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        enemyStatusGO.GetComponent<RectTransform>().pivot = new Vecto`r2(1, 1);
        enemyStatusGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, -20);
        enemyStatusGO.AddComponent<CombatantStatusUI>().Initialize(enemy, "Enemy", uiFont);
    }

    private void CreateEndTurnButton(Canvas canvas, Encounter encounter)
    {
        GameObject buttonGO = new GameObject("EndTurnButton", typeof(RectTransform));
        buttonGO.transform.SetParent(canvas.transform, false);
        buttonGO.AddComponent<Image>();
        Button button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(encounter.OnEndTurnButton);

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
        GameObject textGO = new GameObject(name, typeof(RectTransform));
        textGO.transform.SetParent(parent.transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = content;
        text.font = uiFont != null ? uiFont : Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;
        textGO.GetComponent<RectTransform>().anchoredPosition = position;
        textGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
        return text;
    }
}
