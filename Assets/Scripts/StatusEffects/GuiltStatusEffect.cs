using UnityEngine;

public class GuiltStatusEffect : StatusEffect
{
    public GuiltStatusEffect()
    {
        this.name = "Guilt";
        this.description = "Take 5 damage at the start of your turn.";
        this.duration = 2;
        this.type = StatusEffectType.Debuff;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (owner != null)
        {
            Debug.Log($"{owner.name} suffers from Guilt!");
            owner.TakeEmotionalDamage(5, RhetoricalClass.Vulnerability, null);
        }
    }
}
