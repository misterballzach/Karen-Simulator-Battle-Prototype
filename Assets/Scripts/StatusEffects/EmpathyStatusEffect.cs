using UnityEngine;

public class EmpathyStatusEffect : StatusEffect, IOnDamageTrigger
{
    public EmpathyStatusEffect()
    {
        this.name = "Empathy";
        this.description = "Takes 50% more damage from Vulnerability cards.";
        this.duration = 2;
        this.type = StatusEffectType.Debuff;
    }

    public bool OnTakeDamage(Combatant attacker, ref int damage)
    {
        // This is a limitation. A proper implementation would pass the rhetorical class here.
        Debug.Log("Empathy is active! Increasing damage.");
        damage = Mathf.RoundToInt(damage * 1.5f);
        return true;
    }
}
