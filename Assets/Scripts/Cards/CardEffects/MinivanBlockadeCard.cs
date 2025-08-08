using UnityEngine;

[CreateAssetMenu(fileName = "New Minivan Blockade Card", menuName = "Cards/Minivan Blockade Card")]
public class MinivanBlockadeCard : Card
{
    public int armorAmount;

    public override void Use(Entity user, Entity target)
    {
        if (user != null)
        {
            user.armor += armorAmount;
            Debug.Log($"{user.name} used {this.name} to gain {armorAmount} armor.");
        }
    }
}
