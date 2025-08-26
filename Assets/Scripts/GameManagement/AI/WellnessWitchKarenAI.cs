using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Wellness Witch AI", menuName = "KAREN/AI/Wellness Witch")]
public class WellnessWitchKarenAI : AIProfile
{
    public override System.Tuple<VerbalAbility, Combatant> ChooseAbility(Combatant self, List<Combatant> allies, List<Combatant> enemies, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns)
    {
        if (self.preparedArguments.Count == 0) return new System.Tuple<VerbalAbility, Combatant>(null, null);

        // 1. Prioritize buffing if not already buffed
        bool hasBuff = self.statusEffects.Exists(e => e is SelfBuffStatusEffect);
        if (!hasBuff)
        {
            var buffAbility = self.preparedArguments.FirstOrDefault(a => a is MLMBuffsAbility && self.currentCredibility >= a.cost);
            if (buffAbility != null)
            {
                Debug.Log("AI chooses to buff.");
                return new System.Tuple<VerbalAbility, Combatant>(buffAbility, self);
            }
        }

        // 2. Use other abilities
        var usableAbilities = self.preparedArguments.Where(a => !(a is MLMBuffsAbility) && self.currentCredibility >= a.cost).ToList();
        if(usableAbilities.Count > 0)
        {
            var abilityToUse = usableAbilities[Random.Range(0, usableAbilities.Count)];
            Combatant target = (abilityToUse.rhetoricalClass == RhetoricalClass.Vulnerability) ? self : enemies[0];
            Debug.Log($"AI chooses to use {abilityToUse.name}.");
            return new System.Tuple<VerbalAbility, Combatant>(abilityToUse, target);
        }

        Debug.Log("AI has no valid moves.");
        return new System.Tuple<VerbalAbility, Combatant>(null, null);
    }
}
