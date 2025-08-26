using UnityEngine;

[System.Serializable]
public class ViralEffect : StatusEffect
{
    public ViralEffect()
    {
        name = "Viral";
        description = "This meltdown is being recorded! All post-encounter consequences are doubled.";
        type = StatusEffectType.Debuff;
        duration = 99; // Lasts the whole battle
    }

    public override void Apply(Combatant target)
    {
        base.Apply(target);
        Encounter encounter = Object.FindFirstObjectByType<Encounter>();
        if (encounter != null)
        {
            encounter.isViral = true;
            Debug.Log("The encounter has gone VIRAL!");
        }
    }
}
