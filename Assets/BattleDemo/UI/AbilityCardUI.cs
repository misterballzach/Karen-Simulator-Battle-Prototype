using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityCardUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Text nameText;
    public Text descriptionText;
    public Text costText;

    private VerbalAbility ability;
    private Combatant player;
    private Encounter encounter;

    // Call this after instantiating the card
    public void Setup(VerbalAbility abilityData, Combatant player, Encounter encounter)
    {
        this.ability = abilityData;
        this.player = player;
        this.encounter = encounter;

        if (nameText != null) nameText.text = ability.name;
        if (descriptionText != null) descriptionText.text = ability.description;
        if (costText != null) costText.text = $"Cost: {ability.cost}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ability == null || player == null || encounter == null)
        {
            Debug.LogError("AbilityCardUI is not properly set up!");
            return;
        }

        if (encounter.state != EncounterState.PlayerTurn)
        {
            Debug.Log("Not the player's turn!");
            return;
        }

        if (encounter.ActiveCombatant != player)
        {
            Debug.Log("It's not this combatant's turn!");
            return;
        }

        // For this demo, we'll assume offensive abilities target the first enemy,
        // and other abilities (buffs/healing) target the user.
        Combatant target = null;
        bool isDefensive = ability is DefensiveStanceAbility; // Check if it's the defensive type

        if (isDefensive)
        {
            target = player;
        }
        else
        {
            if (encounter.enemyParty != null && encounter.enemyParty.Count > 0)
            {
                target = encounter.enemyParty[0];
            }
        }

        if (target != null)
        {
            AudioManager.Instance.PlayClickSound();
            encounter.OnAbilityUsed(ability, target);
        }
        else
        {
            Debug.LogWarning($"No valid target found for {ability.name}");
        }
    }
}
