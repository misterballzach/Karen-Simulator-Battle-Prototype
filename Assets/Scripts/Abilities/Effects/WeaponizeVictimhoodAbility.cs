using UnityEngine;

[CreateAssetMenu(fileName = "Weaponize Victimhood", menuName = "KAREN/Abilities/Weaponize Victimhood")]
public class WeaponizeVictimhoodAbility : VerbalAbility
{
    public void Use(Combatant user, Combatant target)
    {
        if (user != null && target != null)
        {
            Debug.Log($"{user.name} uses Weaponized Victimhood, annoying themself but triggering {target.name}!");
            user.AddStatusEffect(new AnnoyedEffect());
            target.AddStatusEffect(new TriggeredEffect());
        }
    }
}
