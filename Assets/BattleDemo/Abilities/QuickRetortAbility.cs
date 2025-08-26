using UnityEngine;

[CreateAssetMenu(fileName = "Quick Retort", menuName = "KAREN/Battle Demo/Quick Retort")]
public class QuickRetortAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} uses Quick Retort on {target.name}!");
            target.TakeEmotionalDamage(damage, rhetoricalClass, user);
        }
    }
}
