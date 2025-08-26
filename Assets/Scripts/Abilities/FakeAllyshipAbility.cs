using UnityEngine;

[CreateAssetMenu(fileName = "Fake Allyship", menuName = "KAREN/Abilities/Fake Allyship")]
public class FakeAllyshipAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses Fake Allyship!");
        user.AnimateAttack(target);
        user.RecoverStamina(healing); // Heals self by a small amount
        target.TakeEmotionalDamage(damage, rhetoricalClass, user); // Deals small damage
    }
}
