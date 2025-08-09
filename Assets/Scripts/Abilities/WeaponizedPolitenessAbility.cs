using UnityEngine;

[CreateAssetMenu(fileName = "Weaponized Politeness", menuName = "KAREN/Abilities/Weaponized Politeness")]
public class WeaponizedPolitenessAbility : VerbalAbility
{
    public WeaponizedPolitenessAbility()
    {
        isDebuff = true;
    }

    public override void Use(Combatant user, Combatant target)
    {
        // "50% chance to stun target for 1 turn if their morale is below 50%."
        float moralePercentage = (float)target.currentEmotionalStamina / target.maxEmotionalStamina;

        if (moralePercentage < 0.5f)
        {
            if (Random.value < 0.5f)
            {
                Debug.Log($"{target.name} is vulnerable and was stunned by weaponized politeness!");
                target.AddStatusEffect(new StunEffect());
            }
            else
            {
                Debug.Log($"{user.name} attempted to stun, but it failed.");
            }
        }
        else
        {
            // Even if it doesn't stun, it should still do *something*. Let's add a small amount of emotional damage.
            Debug.Log($"{user.name} uses Weaponized Politeness, but the target's morale is too high to stun.");
            target.TakeEmotionalDamage(5, rhetoricalClass, user);
        }
    }
}
