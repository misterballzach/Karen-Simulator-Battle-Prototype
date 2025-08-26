using UnityEngine;

[CreateAssetMenu(fileName = "Weaponized Tears", menuName = "KAREN/Abilities/Weaponized Tears")]
public class WeaponizedTearsAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} uses Weaponized Tears!");
            user.AnimateAttack(target);
            target.TakeEmotionalDamage(damage, rhetoricalClass, user);
            target.AddStatusEffect(new GuiltStatusEffect());
        }
    }
}
