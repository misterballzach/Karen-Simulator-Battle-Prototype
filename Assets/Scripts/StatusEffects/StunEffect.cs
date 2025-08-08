using UnityEngine;

[System.Serializable]
public class StunEffect : FlusteredEffect
{
    public StunEffect()
    {
        name = "Stunned";
        description = "This unit is stunned and cannot take their next turn.";
        type = StatusEffectType.Debuff;
        duration = 1;
        missChance = 1.0f; // 100% chance to miss turn
    }
}
