using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Classic Karen AI", menuName = "KAREN/AI/Classic Karen")]
public class ClassicKarenAI : AIProfile
{
    public override Tuple<VerbalAbility, Combatant> ChooseAbility(Combatant self, List<Combatant> allies, List<Combatant> enemies, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns)
    {
        VerbalAbility bestAbility = null;
        Combatant bestTarget = null;
        float bestScore = -1f;

        foreach (VerbalAbility ability in abilities)
        {
            if (cooldowns.ContainsKey(ability)) continue;

            int finalCost = Mathf.Max(0, ability.cost + self.credibilityCostModifier);
            if (self.currentCredibility < finalCost) continue;

            // Determine if the ability is offensive or defensive
            bool isDefensive = ability is FakeCryAbility || ability is MinivanBlockadeAbility || ability is TraumaDumpAbility || (ability is ApplyStatusAbility app && app.effectToApply.type == StatusEffectType.Buff);

            List<Combatant> potentialTargets = isDefensive ? allies : enemies;

            foreach (Combatant potentialTarget in potentialTargets)
            {
                float currentScore = 0f;

                // --- AI Scoring Logic ---
                if (ability is DemandRefundAbility demand)
                {
                    currentScore = demand.emotionalDamage * 1.5f;
                    if (potentialTarget.rhetoricalWeaknesses.Contains(ability.rhetoricalClass)) currentScore *= 2f;
                    if (demand.emotionalDamage >= potentialTarget.currentEmotionalStamina) currentScore += 1000;
                }
                else if (ability is FakeCryAbility cry)
                {
                    currentScore = cry.staminaToRecover;
                    if ((float)potentialTarget.currentEmotionalStamina / potentialTarget.maxEmotionalStamina < 0.4f) currentScore *= 3f;
                }
                else if (ability is ApplyStatusAbility status)
                {
                    if (status.effectToApply.type == StatusEffectType.Debuff)
                    {
                        currentScore = 15;
                        if (status.effectToApply is StunEffect || status.effectToApply is FiredEffect) currentScore = 50;
                        if (potentialTarget.statusEffects.Exists(e => e.GetType() == status.effectToApply.GetType())) currentScore = 0;
                    }
                    else // It's a buff
                    {
                        currentScore = 20; // Buffs are generally good
                    }
                }
                else if (ability is MinivanBlockadeAbility blockade)
                {
                    currentScore = blockade.armorAmount;
                    if (potentialTarget.armor < 10) currentScore *= 1.5f;
                }
                else if (ability is TraumaDumpAbility)
                {
                    if (self.preparedArguments.Count < 2) currentScore = 20;
                    else currentScore = 5;
                }

                // Add basic logic for Vulnerability
                if(ability.rhetoricalClass == RhetoricalClass.Vulnerability)
                {
                    // AI is a classic Karen, she avoids vulnerability
                    currentScore *= 0.1f;
                }

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestAbility = ability;
                    bestTarget = potentialTarget;
                }
            }
        }
        return new Tuple<VerbalAbility, Combatant>(bestAbility, bestTarget);
    }
}
