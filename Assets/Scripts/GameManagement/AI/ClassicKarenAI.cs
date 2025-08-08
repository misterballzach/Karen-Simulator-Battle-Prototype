using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Classic Karen AI", menuName = "KAREN/AI/Classic Karen")]
public class ClassicKarenAI : AIProfile
{
    public override Tuple<VerbalAbility, Combatant> ChooseAbility(Combatant self, Combatant target, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns)
    {
        VerbalAbility bestAbility = null;
        Combatant bestTarget = null;
        float bestScore = -1f;

        foreach (VerbalAbility ability in abilities)
        {
            if (cooldowns.ContainsKey(ability)) continue;

            int finalCost = Mathf.Max(0, ability.cost + self.credibilityCostModifier);
            if (self.currentCredibility < finalCost) continue;

            float currentScore = 0f;
            Combatant currentTarget = target; // Default to enemy

            // --- AI Scoring Logic ---
            if (ability is DemandRefundAbility demand)
            {
                currentScore = demand.emotionalDamage * 1.5f;
                if (target.rhetoricalWeaknesses.Contains(ability.rhetoricalClass)) currentScore *= 2f;
                if (demand.emotionalDamage >= target.currentEmotionalStamina) currentScore += 1000;
            }
            else if (ability is FakeCryAbility cry)
            {
                currentTarget = self; // Target self for healing
                currentScore = cry.staminaToRecover;
                if ((float)self.currentEmotionalStamina / self.maxEmotionalStamina < 0.4f) currentScore *= 3f;
            }
            else if (ability is ApplyStatusAbility status)
            {
                // Simple logic: assume debuffs target enemy, buffs target self
                if (status.effectToApply.type == StatusEffectType.Debuff)
                {
                    currentTarget = target;
                    currentScore = 15;
                    if (status.effectToApply is StunEffect || status.effectToApply is FiredEffect) currentScore = 50;
                    if (target.statusEffects.Exists(e => e.GetType() == status.effectToApply.GetType())) currentScore = 0;
                }
                else // It's a buff
                {
                    currentTarget = self;
                    currentScore = 20; // Buffs are generally good
                }
            }
            else if (ability is MinivanBlockadeAbility blockade)
            {
                currentTarget = self;
                currentScore = blockade.armorAmount;
                if (self.armor < 10) currentScore *= 1.5f;
            }
            else if (ability is TraumaDumpAbility)
            {
                currentTarget = self;
                if (self.preparedArguments.Count < 2) currentScore = 20;
                else currentScore = 5;
            }

            // Add basic logic for Vulnerability
            if(ability.rhetoricalClass == RhetoricalClass.Vulnerability)
            {
                // AI is a classic Karen, she avoids vulnerability
                currentScore = 0.1f;
            }


            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestAbility = ability;
                bestTarget = currentTarget;
            }
        }
        return new Tuple<VerbalAbility, Combatant>(bestAbility, bestTarget);
    }
}
