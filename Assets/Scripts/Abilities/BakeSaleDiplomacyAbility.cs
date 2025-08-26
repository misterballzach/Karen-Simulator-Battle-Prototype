using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Bake Sale Diplomacy", menuName = "KAREN/Abilities/Bake Sale Diplomacy")]
public class BakeSaleDiplomacyAbility : VerbalAbility
{
    public BakeSaleDiplomacyAbility()
    {
        healing = 20;
    }

    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses Bake Sale Diplomacy!");
        if (user.currentEncounter != null)
        {
            foreach (var ally in user.currentEncounter.playerParty)
            {
                // Restore morale (Emotional Stamina)
                ally.RecoverStamina(healing);

                // Remove one debuff
                if (ally.statusEffects.Count > 0)
                {
                    // For now, we'll remove the last-applied status effect that is a debuff.
                    StatusEffect effectToRemove = ally.statusEffects.LastOrDefault(e => e.type == StatusEffectType.Debuff);
                    if (effectToRemove != null)
                    {
                        effectToRemove.Remove();
                        ally.statusEffects.Remove(effectToRemove);
                        Debug.Log($"{ally.name} feels better. Removed debuff: {effectToRemove.GetType().Name}");
                    }
                }
            }
        }
        else
        {
            // Fallback for testing or edge cases
            user.RecoverStamina(healing);
        }
    }
}
