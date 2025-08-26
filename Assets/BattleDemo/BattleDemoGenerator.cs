using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class BattleDemoGenerator : MonoBehaviour
{
    [Header("Assets (Drag from Project)")]
    public Font uiFont; // Optional: for better text rendering
    public Sprite playerBodySprite;
    public Sprite playerFaceSprite;
    public Sprite enemyBodySprite;
    public Sprite enemyFaceSprite;
    public Sprite endTurnButtonSprite;
    public Sprite cardBackgroundSprite;
    public Sprite sliderBackgroundSprite;
    public Sprite sliderFillSprite;

    private Encounter encounter;

    void Start()
    {
        // --- Create Core Systems ---
        CreateEventSystem();
        Canvas canvas = CreateCanvas();
        GameObject encounterGO = new GameObject("EncounterManager");
        encounter = encounterGO.AddComponent<Encounter>();

        // --- Load Sprites ---
        Sprite aggressionIcon = Resources.Load<Sprite>("UI/icon_cross");
        Sprite manipulationIcon = Resources.Load<Sprite>("UI/icon_circle");
        Sprite vulnerabilityIcon = Resources.Load<Sprite>("UI/icon_checkmark");
        Sprite delusionIcon = Resources.Load<Sprite>("UI/icon_square");

        // --- Create Player & Enemy Abilities ---
        var abilities = new Dictionary<string, VerbalAbility>();

        abilities["Quick Retort"] = ScriptableObject.CreateInstance<QuickRetortAbility>();
        abilities["Quick Retort"].name = "Quick Retort";
        abilities["Quick Retort"].description = "A swift, cutting remark.";
        abilities["Quick Retort"].cost = 10;
        abilities["Quick Retort"].damage = 15;
        abilities["Quick Retort"].rhetoricalClass = RhetoricalClass.Aggression;
        abilities["Quick Retort"].artwork = aggressionIcon;

        abilities["Defensive Stance"] = ScriptableObject.CreateInstance<DefensiveStanceAbility>();
        abilities["Defensive Stance"].name = "Defensive Stance";
        abilities["Defensive Stance"].description = "Brace for impact, gaining 15 Armor.";
        abilities["Defensive Stance"].cost = 10;
        ((DefensiveStanceAbility)abilities["Defensive Stance"]).armorGain = 15;
        abilities["Defensive Stance"].rhetoricalClass = RhetoricalClass.Vulnerability;
        abilities["Defensive Stance"].artwork = vulnerabilityIcon;

        abilities["Entitled Rage"] = ScriptableObject.CreateInstance<EntitledRageAbility>();
        abilities["Entitled Rage"].name = "Entitled Rage";
        abilities["Entitled Rage"].description = "Deal 25 damage and gain 15 Entitlement.";
        abilities["Entitled Rage"].cost = 15;
        abilities["Entitled Rage"].damage = 25;
        abilities["Entitled Rage"].rhetoricalClass = RhetoricalClass.Aggression;
        abilities["Entitled Rage"].artwork = aggressionIcon;

        abilities["Weaponized Tears"] = ScriptableObject.CreateInstance<WeaponizedTearsAbility>();
        abilities["Weaponized Tears"].name = "Weaponized Tears";
        abilities["Weaponized Tears"].description = "Deal 5 damage and inflict 2 turns of Guilt.";
        abilities["Weaponized Tears"].cost = 10;
        abilities["Weaponized Tears"].damage = 5;
        abilities["Weaponized Tears"].rhetoricalClass = RhetoricalClass.Manipulation;
        abilities["Weaponized Tears"].artwork = manipulationIcon;

        abilities["Sincere Apology"] = ScriptableObject.CreateInstance<SincereApologyAbility>();
        abilities["Sincere Apology"].name = "Sincere Apology";
        abilities["Sincere Apology"].description = "Heal 20 Stamina and apply Empathy to the target for 2 turns.";
        abilities["Sincere Apology"].cost = 20;
        abilities["Sincere Apology"].healing = 20;
        abilities["Sincere Apology"].rhetoricalClass = RhetoricalClass.Vulnerability;
        abilities["Sincere Apology"].artwork = vulnerabilityIcon;

        abilities["Fake Allyship"] = ScriptableObject.CreateInstance<FakeAllyshipAbility>();
        abilities["Fake Allyship"].name = "Fake Allyship";
        abilities["Fake Allyship"].description = "Heal 5 Stamina, deal 10 damage.";
        abilities["Fake Allyship"].cost = 10;
        abilities["Fake Allyship"].damage = 10;
        abilities["Fake Allyship"].healing = 5;
        abilities["Fake Allyship"].rhetoricalClass = RhetoricalClass.Manipulation;
        abilities["Fake Allyship"].artwork = manipulationIcon;

        abilities["Shared Trauma"] = ScriptableObject.CreateInstance<SharedTraumaAbility>();
        abilities["Shared Trauma"].name = "Shared Trauma";
        abilities["Shared Trauma"].description = "Both take damage, but the enemy takes more. Both gain 10 Insight.";
        abilities["Shared Trauma"].cost = 10;
        abilities["Shared Trauma"].damage = 20;
        abilities["Shared Trauma"].rhetoricalClass = RhetoricalClass.Vulnerability;
        abilities["Shared Trauma"].artwork = vulnerabilityIcon;

        abilities["MLM Buff"] = ScriptableObject.CreateInstance<MLMBuffsAbility>();
        abilities["MLM Buff"].name = "MLM Buff";
        abilities["MLM Buff"].description = "Apply 'Essential Oil Vigor' to self for 2 turns.";
        abilities["MLM Buff"].cost = 15;
        abilities["MLM Buff"].rhetoricalClass = RhetoricalClass.Delusion;
        abilities["MLM Buff"].artwork = delusionIcon;

        abilities["Facebook Misinformation"] = ScriptableObject.CreateInstance<FacebookMisinformationAbility>();
        abilities["Facebook Misinformation"].name = "Facebook Misinformation";
        abilities["Facebook Misinformation"].description = "Applies 'Flustered' to the target.";
        abilities["Facebook Misinformation"].cost = 10;
        abilities["Facebook Misinformation"].rhetoricalClass = RhetoricalClass.Delusion;
        abilities["Facebook Misinformation"].artwork = delusionIcon;

        // --- Create AI ---
        WellnessWitchKarenAI wellnessWitchAI = ScriptableObject.CreateInstance<WellnessWitchKarenAI>();

        // --- Create Combatants ---
        Combatant player = CreateCombatant("Player", new Vector3(-3.5f, 0, 0), Faction.Player, playerBodySprite, playerFaceSprite);
        player.verbalLoadout = new List<VerbalAbility> {
            abilities["Quick Retort"],
            abilities["Defensive Stance"],
            abilities["Entitled Rage"],
            abilities["Weaponized Tears"],
            abilities["Sincere Apology"],
            abilities["Fake Allyship"],
            abilities["Shared Trauma"]
        };

        Combatant enemy = CreateCombatant("Wellness Witch Karen", new Vector3(3.5f, 0, 0), Faction.Enemy, enemyBodySprite, enemyFaceSprite);
        enemy.aiProfile = wellnessWitchAI;
        enemy.verbalLoadout = new List<VerbalAbility> {
            abilities["Quick Retort"],
            abilities["MLM Buff"],
            abilities["Facebook Misinformation"],
            abilities["Quick Retort"],
            abilities["MLM Buff"]
        };

        // --- Final Setup ---
        encounter.playerParty = new List<Combatant> { player };
        encounter.enemyParty = new List<Combatant> { enemy };
        player.currentEncounter = encounter;
        enemy.currentEncounter = encounter;

        for (int i = 0; i < 5; i++)
        {
            player.PrepareArgument();
            enemy.PrepareArgument();
        }

        ArgumentHandUI handUI = CreateArgumentHandUI(canvas, player, encounter);
        encounter.playerHandUI = handUI;
        CreateStatusUI(canvas, player, enemy);
        CreateEndTurnButton(canvas, encounter);

        Debug.Log("Battle Demo Generated. Starting encounter...");
    }

    #region Helper Methods for UI and Object Creation
    private Combatant CreateCombatant(string name, Vector3 position, Faction faction, Sprite bodySprite, Sprite faceSprite)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;
        go.transform.localScale = new Vector3(3, 3, 3);

        // Body
        GameObject bodyGO = new GameObject("Body");
        bodyGO.transform.SetParent(go.transform, false);
        SpriteRenderer bodySr = bodyGO.AddComponent<SpriteRenderer>();
        bodySr.sprite = bodySprite;

        // Face
        GameObject faceGO = new GameObject("Face");
        faceGO.transform.SetParent(go.transform, false);
        SpriteRenderer faceSr = faceGO.AddComponent<SpriteRenderer>();
        faceSr.sprite = faceSprite;
        faceSr.sortingOrder = 1; // Ensure face is drawn on top of body

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

        GameObject cardTemplate = new GameObject("CardTemplate");
        cardTemplate.transform.SetParent(handContainerGO.transform);
        Image cardImage = cardTemplate.AddComponent<Image>();
        if (cardBackgroundSprite != null)
        {
            cardImage.sprite = cardBackgroundSprite;
            cardImage.type = Image.Type.Sliced;
        }
        else
        {
            cardImage.color = new Color(0.9f, 0.9f, 0.9f);
        }
        RectTransform cardRect = cardTemplate.GetComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(120, 160);

        GameObject artworkGO = new GameObject("Artwork");
        artworkGO.transform.SetParent(cardTemplate.transform, false);
        Image artworkImage = artworkGO.AddComponent<Image>();
        RectTransform artworkRect = artworkGO.GetComponent<RectTransform>();
        artworkRect.anchorMin = new Vector2(0, 0.5f);
        artworkRect.anchorMax = new Vector2(1, 1);
        artworkRect.pivot = new Vector2(0.5f, 0.5f);
        artworkRect.offsetMin = new Vector2(10, 50);
        artworkRect.offsetMax = new Vector2(-10, -10);

        Text nameText = CreateText(cardTemplate, "NameText", "Ability Name", 14, new Vector2(0, 25));
        Text descText = CreateText(cardTemplate, "DescriptionText", "Description", 10, new Vector2(0, -20));
        Text costText = CreateText(cardTemplate, "CostText", "Cost: X", 12, new Vector2(0, -65));

        AbilityCardUI cardUI = cardTemplate.AddComponent<AbilityCardUI>();
        cardUI.artworkImage = artworkImage;
        cardUI.nameText = nameText;
        cardUI.descriptionText = descText;
        cardUI.costText = costText;

        handUI.abilityPrefab = cardTemplate;
        cardTemplate.SetActive(false);

        handUI.gameObject.AddComponent<CustomHandUIUpdater>().Setup(handUI, player, currentEncounter, cardTemplate);

        return handUI;
    }

    private void CreateStatusUI(Canvas canvas, Combatant player, Combatant enemy)
    {
        GameObject playerStatusGO = new GameObject("PlayerStatus");
        playerStatusGO.transform.SetParent(canvas.transform, false);
        RectTransform playerRect = playerStatusGO.AddComponent<RectTransform>();
        playerRect.anchorMin = new Vector2(0, 1);
        playerRect.anchorMax = new Vector2(0, 1);
        playerRect.pivot = new Vector2(0, 1);
        playerRect.anchoredPosition = new Vector2(20, -20);
        CombatantStatusUI playerStatusUI = playerStatusGO.AddComponent<CombatantStatusUI>();
        playerStatusUI.sliderBackgroundSprite = sliderBackgroundSprite;
        playerStatusUI.sliderFillSprite = sliderFillSprite;
        playerStatusUI.Initialize(player, "Player");

        GameObject enemyStatusGO = new GameObject("EnemyStatus");
        enemyStatusGO.transform.SetParent(canvas.transform, false);
        RectTransform enemyRect = enemyStatusGO.AddComponent<RectTransform>();
        enemyRect.anchorMin = new Vector2(1, 1);
        enemyRect.anchorMax = new Vector2(1, 1);
        enemyRect.pivot = new Vector2(1, 1);
        enemyRect.anchoredPosition = new Vector2(-20, -20);
        CombatantStatusUI enemyStatusUI = enemyStatusGO.AddComponent<CombatantStatusUI>();
        enemyStatusUI.sliderBackgroundSprite = sliderBackgroundSprite;
        enemyStatusUI.sliderFillSprite = sliderFillSprite;
        enemyStatusUI.Initialize(enemy, "Enemy");
    }

    private void CreateEndTurnButton(Canvas canvas, Encounter currentEncounter)
    {
        GameObject buttonGO = new GameObject("EndTurnButton");
        buttonGO.transform.SetParent(canvas.transform, false);
        Image buttonImage = buttonGO.AddComponent<Image>();
        if (endTurnButtonSprite != null)
        {
            buttonImage.sprite = endTurnButtonSprite;
            buttonImage.type = Image.Type.Sliced;
        }
        Button button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(currentEncounter.OnEndTurnButton);
        button.onClick.AddListener(AudioManager.Instance.PlayClickSound);

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
        text.font = uiFont != null ? uiFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;
        RectTransform rectTransform = textGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(100, 100);
        return text;
    }
    #endregion
}
