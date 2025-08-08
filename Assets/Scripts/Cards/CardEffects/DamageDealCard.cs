using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Deal Card", menuName = "Cards/Damage Deal Card")]
public class DamageDealCard : Card
{
    public int damageAmount;

    public override void Use(Entity target)
    {
        if (target != null)
        {
            target.TakeDamage(damageAmount, this.element);
            Debug.Log($"{this.name} dealt {damageAmount} {this.element.ToString()} damage to {target.name}.");
        }
    }
}
