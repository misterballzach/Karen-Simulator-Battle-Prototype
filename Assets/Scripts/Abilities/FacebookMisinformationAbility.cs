using UnityEngine;

[CreateAssetMenu(fileName = "Facebook Misinformation", menuName = "KAREN/Abilities/Facebook Misinformation")]
public class FacebookMisinformationAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses Facebook Misinformation!");
        target.AddStatusEffect(new FlusteredEffect());
    }
}
