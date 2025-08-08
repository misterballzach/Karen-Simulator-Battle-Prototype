using UnityEngine;

[CreateAssetMenu(fileName = "Misquote Policy", menuName = "KAREN/Abilities/Misquote Policy")]
public class MisquotePolicyAbility : VerbalAbility
{
    public void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} misquotes policy at {target.name}, leaving them flustered!");
            target.AddStatusEffect(new FlusteredEffect());
        }
    }
}
