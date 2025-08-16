using UnityEngine;

[CreateAssetMenu(fileName = "Demand Refund", menuName = "KAREN/Abilities/Demand Refund")]
public class DemandRefundAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            int modifiedDamage = Mathf.RoundToInt(damage * user.outgoingDamageModifier);
            target.TakeEmotionalDamage(modifiedDamage, this.rhetoricalClass, user);
            Debug.Log($"{user.name} used {this.name}, dealing {modifiedDamage} emotional damage to {target.name}.");
        }
    }
}
