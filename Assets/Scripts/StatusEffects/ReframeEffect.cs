using UnityEngine;

[System.Serializable]
public class ReframeEffect : StatusEffect, IOnDamageTrigger
{
    public ReframeEffect()
    {
        name = "Reframe";
        description = "The next attack against this unit will be cancelled, and the attacker will be made Insecure.";
        type = StatusEffectType.Buff;
        duration = 1; // Lasts for one turn, or until triggered
    }

    public bool OnTakeDamage(Combatant attacker, ref int damage)
    {
        if (attacker != null)
        {
            Debug.Log($"{owner.name} reframes the attack from {attacker.name}!");
            attacker.AddStatusEffect(new InsecureEffect());
        }

        // Consume the effect once it triggers
        duration = 0;

        // Return false to cancel the incoming damage
        return false;
    }
}
