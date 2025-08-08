using UnityEngine;

[System.Serializable]
public class PoisonEffect : StatusEffect
{
    public int damagePerTurn;

    public PoisonEffect()
    {
        name = "Poison";
        description = "Deals damage at the start of the turn.";
        type = StatusEffectType.Debuff;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (owner != null)
        {
            owner.TakeDamage(damagePerTurn, Element.Neutral);
            Debug.Log($"{owner.name} takes {damagePerTurn} damage from poison.");
        }
    }
}
