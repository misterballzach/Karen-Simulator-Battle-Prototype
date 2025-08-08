using UnityEngine;

[CreateAssetMenu(fileName = "Go Viral Consequence", menuName = "KAREN/Consequences/Go Viral")]
public class GoViralConsequence : Consequence
{
    public override void Trigger(Combatant user)
    {
        Debug.Log($"OH NO! {user.name}'s meltdown is going viral!");
        user.AddStatusEffect(new ViralEffect());
    }
}
