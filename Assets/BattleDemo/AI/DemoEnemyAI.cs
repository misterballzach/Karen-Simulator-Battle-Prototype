using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "Demo Enemy AI", menuName = "KAREN/Battle Demo/Demo Enemy AI")]
public class DemoEnemyAI : AIProfile
{
    public override Tuple<VerbalAbility, Combatant> ChooseAbility(Combatant self, List<Combatant> allies, List<Combatant> enemies, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns)
    {
        VerbalAbility chosenAbility = null;
        Combatant target = null;

        // Find the first available attack ability
        var attackAbility = abilities.FirstOrDefault(a => a is QuickRetortAbility && !cooldowns.ContainsKey(a) && self.currentCredibility >= a.cost);
        if (attackAbility != null && enemies.Count > 0)
        {
            chosenAbility = attackAbility;
            target = enemies[0]; // Target the first enemy
            Debug.Log($"AI chooses to attack with {chosenAbility.name}");
            return new Tuple<VerbalAbility, Combatant>(chosenAbility, target);
        }

        // If no attack is possible, find the first available defensive ability
        var defensiveAbility = abilities.FirstOrDefault(a => a is DefensiveStanceAbility && !cooldowns.ContainsKey(a) && self.currentCredibility >= a.cost);
        if (defensiveAbility != null)
        {
            chosenAbility = defensiveAbility;
            target = self; // Target self for defensive ability
            Debug.Log($"AI chooses to defend with {chosenAbility.name}");
            return new Tuple<VerbalAbility, Combatant>(chosenAbility, target);
        }

        Debug.Log("AI has no valid moves.");
        return new Tuple<VerbalAbility, Combatant>(null, null);
    }
}
