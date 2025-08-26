using UnityEngine;

[CreateAssetMenu(fileName = "Drop Husband's Job Title", menuName = "KAREN/Abilities/Drop Husband's Job Title")]
public class DropHusbandsJobTitleAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} drops their husband's job title on {target.name}!");
            if (target.level <= user.level)
            {
                Debug.Log($"It's super effective! {target.name} is stunned!");
                target.AddStatusEffect(new StunEffect());
            }
            else
            {
                Debug.Log($"...but it had no effect.");
            }
        }
    }
}
