using UnityEngine;

[CreateAssetMenu(fileName = "Entitled Rage", menuName = "KAREN/Abilities/Entitled Rage")]
public class EntitledRageAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} uses Entitled Rage!");
            user.AnimateAttack(target);
            target.TakeEmotionalDamage(damage, rhetoricalClass, user);
            user.GainEntitlement(15);
        }
    }
}
