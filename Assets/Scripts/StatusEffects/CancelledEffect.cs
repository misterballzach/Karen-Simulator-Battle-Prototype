using UnityEngine;

[System.Serializable]
public class CancelledEffect : StatusEffect
{
    public CancelledEffect()
    {
        name = "Cancelled";
        description = "This unit has been cancelled. They cannot use Aggression abilities.";
        type = StatusEffectType.Debuff;
        duration = 4;
    }
}
