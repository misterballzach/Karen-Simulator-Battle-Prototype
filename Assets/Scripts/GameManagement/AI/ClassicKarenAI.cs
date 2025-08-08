using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Classic Karen AI", menuName = "KAREN/AI/Classic Karen")]
public class ClassicKarenAI : AIProfile
{
    public override VerbalAbility ChooseAbility(Combatant self, Combatant target, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns)
    {
        VerbalAbility bestAbility = null;
        float bestScore = -1f;

        foreach (VerbalAbility ability in abilities)
        {
            if (cooldowns.ContainsKey(ability)) continue;

            int finalCost = Mathf.Max(0, ability.cost + self.credibilityCostModifier);
            if (self.currentCredibility < finalCost) continue;

            float currentScore = 0f;

            if (ability is DemandRefundAbility demand)
            {
                currentScore = demand.emotionalDamage * 1.5f;
                if (target.rhetoricalWeaknesses.Contains(ability.rhetoricalClass)) currentScore *= 2f;
                if (demand.emotionalDamage >= target.currentEmotionalStamina) currentScore += 1000;
            }
            else if (ability is FakeCryAbility cry)
            {
                currentScore = cry.staminaToRecover;
                if ((float)self.currentEmotionalStamina / self.maxEmotionalStamina < 0.4f) currentScore *= 3f;
            }
            else if (ability is ApplyStatusAbility status)
            {
                currentScore = 15;
                if (status.effectToApply is StunEffect || status.effectToApply is FiredEffect) currentScore = 50;
                if (target.statusEffects.Exists(e => e.GetType() == status.effectToApply.GetType())) currentScore = 0;
            }
            else if (ability is MinivanBlockadeAbility blockade)
            {
                currentScore = blockade.armorAmount;
                if (self.armor < 10) currentScore *= 1.5f;
            }
            else if (ability is TraumaDumpAbility)
            {
                if (self.preparedArguments.Count < 2) currentScore = 20;
                else currentScore = 5;
            }

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestAbility = ability;
            }
        }
        return bestAbility;
    }
}
