using UnityEngine;

[CreateAssetMenu(fileName = "Defensive Stance", menuName = "KAREN/Battle Demo/Defensive Stance")]
public class DefensiveStanceAbility : VerbalAbility
{
    public int armorGain = 15;

    public override void Use(Combatant user, Combatant target)
    {
        Debug.Log($"{user.name} uses Defensive Stance!");
        user.armor += armorGain;
    }
}
