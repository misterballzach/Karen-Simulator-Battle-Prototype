using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Card", menuName = "Cards/Healing Card")]
public class HealingCard : Card
{
    public int healAmount;

    public override void Use(Entity user, Entity target)
    {
        if (target != null)
        {
            target.Heal(healAmount);
            Debug.Log($"{this.name} healed {target.name} for {healAmount} health.");
        }
    }
}
