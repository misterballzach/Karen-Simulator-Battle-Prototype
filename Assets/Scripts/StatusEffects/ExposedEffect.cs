using UnityEngine;

[System.Serializable]
public class ExposedEffect : StatusEffect
{
    public ExposedEffect()
    {
        name = "Exposed";
        description = "This unit's hypocrisy has been exposed. They cannot use Manipulation or Delusion abilities for 3 turns.";
        type = StatusEffectType.Debuff;
        duration = 3;
        BlockedRhetoric.Add(RhetoricalClass.Manipulation);
        BlockedRhetoric.Add(RhetoricalClass.Delusion);
    }
}
