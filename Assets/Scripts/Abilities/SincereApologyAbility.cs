using UnityEngine;

[CreateAssetMenu(fileName = "Sincere Apology", menuName = "KAREN/Abilities/Sincere Apology")]
public class SincereApologyAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses Sincere Apology.");
        user.RecoverStamina(healing);
        if (target != null)
            target.AddStatusEffect(new EmpathyStatusEffect());
    }
}
