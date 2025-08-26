using UnityEngine;

[CreateAssetMenu(fileName = "MLM Buff", menuName = "KAREN/Abilities/MLM Buff")]
public class MLMBuffsAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses MLM Buffs!");
        user.AddStatusEffect(new SelfBuffStatusEffect());
    }
}
