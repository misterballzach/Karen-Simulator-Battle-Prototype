using UnityEngine;

[System.Serializable]
public class CopsCalledEffect : FlusteredEffect
{
    public int damagePerTurn = 10;

    public CopsCalledEffect()
    {
        name = "Cops Called";
        description = "The cops have been called! This unit is stunned and takes emotional damage each turn.";
        type = StatusEffectType.Debuff;
        duration = 2;
        missChance = 1.0f; // 100% chance to miss turn
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (owner != null)
        {
            owner.TakeEmotionalDamage(damagePerTurn, RhetoricalClass.Aggression, null);
            Debug.Log($"{owner.name} takes {damagePerTurn} emotional damage from the ongoing legal battle.");
        }
    }
}
