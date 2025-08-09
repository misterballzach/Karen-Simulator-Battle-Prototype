using UnityEngine;

[System.Serializable]
public class SnarkyRetortEffect : StatusEffect, IOnDamageTrigger
{
    private float counterDamageMultiplier = 0.5f;

    public SnarkyRetortEffect()
    {
        name = "Snarky Retort";
        description = "If attacked, instantly return damage equal to 50% of received damage.";
        type = StatusEffectType.Buff;
        duration = 1; // Lasts for one turn
    }

    public bool OnTakeDamage(Combatant attacker, ref int damage)
    {
        if (attacker != null)
        {
            int counterDamage = Mathf.RoundToInt(damage * counterDamageMultiplier);
            Debug.Log($"{owner.name} delivers a snarky retort to {attacker.name}, dealing {counterDamage} emotional damage!");
            // The rhetorical class of the counter-attack could be considered 'Vulnerability' or a new 'Retort' class.
            // For now, let's use the owner's class or a neutral one.
            attacker.TakeEmotionalDamage(counterDamage, RhetoricalClass.Vulnerability, owner);
        }
        return true; // Don't cancel the original damage
    }
}
