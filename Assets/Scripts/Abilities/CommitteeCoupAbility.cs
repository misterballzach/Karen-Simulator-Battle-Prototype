using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Committee Coup", menuName = "KAREN/Abilities/Committee Coup")]
public class CommitteeCoupAbility : VerbalAbility
{
    public CommitteeCoupAbility()
    {
        // This is an ultimate ability, so it should have a high cost or special requirement.
        // For now, let's give it a high credibility cost.
        cost = 100;
    }

    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} initiates a Committee Coup against {target.name}!");

        // Find all buffs on the target
        List<StatusEffect> buffsToSteal = target.statusEffects
            .Where(e => e.type == StatusEffectType.Buff)
            .ToList();

        if (buffsToSteal.Count == 0)
        {
            Debug.Log("But the target has no buffs to steal.");
            return;
        }

        Debug.Log($"Stealing {buffsToSteal.Count} buffs...");

        foreach (var buff in buffsToSteal)
        {
            // 1. Clean up the effect from the original owner
            buff.Remove();
            target.statusEffects.Remove(buff);

            // 2. Set the duration and apply to the new owner
            buff.duration = 3;
            user.AddStatusEffect(buff); // AddStatusEffect calls Apply internally

            Debug.Log($"Stole buff: {buff.name}");
        }
    }
}
