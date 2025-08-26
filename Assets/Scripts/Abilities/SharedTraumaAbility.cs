using UnityEngine;

[CreateAssetMenu(fileName = "Shared Trauma", menuName = "KAREN/Abilities/Shared Trauma")]
public class SharedTraumaAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} shares trauma.");
        user.AnimateAttack(target);
        user.TakeEmotionalDamage(Mathf.RoundToInt(damage * 0.5f), rhetoricalClass, null);
        target.TakeEmotionalDamage(damage, rhetoricalClass, user);
        user.GainInsight(10);
        target.GainInsight(10);
    }
}
