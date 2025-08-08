using UnityEngine;

[CreateAssetMenu(fileName = "Minivan Blockade", menuName = "KAREN/Abilities/Minivan Blockade")]
public class MinivanBlockadeAbility : VerbalAbility
{
    public int armorAmount;

    public override void Use(Combatant user, Combatant target)
    {
        if (user != null)
        {
            user.armor += armorAmount;
            Debug.Log($"{user.name} used {this.name} to gain {armorAmount} armor.");
        }
    }
}
