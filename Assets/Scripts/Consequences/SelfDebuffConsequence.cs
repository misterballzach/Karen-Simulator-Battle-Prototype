using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Self Debuff Consequence", menuName = "KAREN/Consequences/Self Debuff")]
public class SelfDebuffConsequence : Consequence
{
    public override void Trigger(Combatant user)
    {
        Debug.Log($"OH NO! {user.name}'s argument backfired!");

        // Apply a random, simple debuff
        List<StatusEffect> possibleDebuffs = new List<StatusEffect>
        {
            new AnnoyedEffect(),
            new FlusteredEffect()
        };

        int randomIndex = Random.Range(0, possibleDebuffs.Count);
        user.AddStatusEffect(possibleDebuffs[randomIndex]);
    }
}
