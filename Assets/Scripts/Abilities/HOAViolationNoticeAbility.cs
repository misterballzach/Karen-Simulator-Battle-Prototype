using UnityEngine;

[CreateAssetMenu(fileName = "HOA Violation Notice", menuName = "KAREN/Abilities/HOA Violation Notice")]
public class HOAViolationNoticeAbility : VerbalAbility
{
    public HOAViolationNoticeAbility()
    {
        isDebuff = true;
    }

    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} issues an HOA Violation Notice to {target.name}!");
        target.AddStatusEffect(new ConfidenceLossDebuff());
    }
}
