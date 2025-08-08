using UnityEngine;

[System.Serializable]
public class FlusteredEffect : StatusEffect
{
    [Range(0, 1)]
    public float missChance = 0.5f;

    public FlusteredEffect()
    {
        name = "Flustered";
        description = "This unit is flustered and has a chance to skip their turn.";
        type = StatusEffectType.Debuff;
    }

    public bool CheckIfTurnSkipped()
    {
        bool skipped = Random.value < missChance;
        if (skipped)
        {
            Debug.Log($"{owner.name} is flustered and skipped their turn!");
        }
        return skipped;
    }
}
