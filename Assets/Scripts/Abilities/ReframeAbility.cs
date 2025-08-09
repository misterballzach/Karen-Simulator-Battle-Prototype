using UnityEngine;

[CreateAssetMenu(fileName = "Reframe", menuName = "KAREN/Abilities/Reframe")]
public class ReframeAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        // This ability applies a buff to the user.
        Debug.Log($"{user.name} prepares to Reframe the narrative.");
        user.AddStatusEffect(new ReframeEffect());
    }
}
