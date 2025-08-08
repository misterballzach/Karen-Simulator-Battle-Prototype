using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityClickHandler : MonoBehaviour, IPointerClickHandler
{
    public VerbalAbility abilityData;
    private Encounter encounter;

    void Start()
    {
        encounter = FindObjectOfType<Encounter>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (encounter != null && encounter.state == EncounterState.PlayerTurn)
        {
            // In a real game, a targeting system would be needed.
            // For now, assume the first enemy is the target for non-self-target abilities.
            Combatant target = encounter.enemy;
            // A more robust system would check the ability's intended target type.

            encounter.OnAbilityUsed(abilityData, target);
        }
    }
}
