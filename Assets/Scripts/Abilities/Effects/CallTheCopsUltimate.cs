using UnityEngine;

[CreateAssetMenu(fileName = "Call The Cops", menuName = "KAREN/Abilities/ULTIMATE - Call The Cops")]
public class CallTheCopsUltimate : VerbalAbility
{
    public CallTheCopsUltimate()
    {
        name = "Call The Cops";
        cost = 0; // No credibility cost
        // This ability uses the PassiveAggressiveMeter instead
    }

    public override void Use(Combatant user, Combatant target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} is calling the cops on {target.name}!");
            target.AddStatusEffect(new CopsCalledEffect());
        }
    }
}
