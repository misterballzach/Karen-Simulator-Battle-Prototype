using UnityEngine;

[CreateAssetMenu(fileName = "Neighborhood Watch", menuName = "KAREN/Abilities/Neighborhood Watch")]
public class NeighborhoodWatchAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} starts the Neighborhood Watch!");
        if (user.currentEncounter != null)
        {
            foreach (var ally in user.currentEncounter.playerParty)
            {
                ally.AddStatusEffect(new ObservationBuff());
                Debug.Log($"{ally.name} joins the Neighborhood Watch.");
            }
        }
        else
        {
            // Fallback for testing or edge cases
            user.AddStatusEffect(new ObservationBuff());
        }
    }
}
