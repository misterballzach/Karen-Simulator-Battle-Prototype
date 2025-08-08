using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Deal Card", menuName = "Cards/Damage Deal Card")]
public class DamageDealCard : Card
{
    public int damageAmount;

    public override void Use(Entity user, Entity target)
    {
        if (target != null)
        {
            int modifiedDamage = Mathf.RoundToInt(damageAmount * user.outgoingDamageModifier);
            target.TakeDamage(modifiedDamage, this.element);
            Debug.Log($"{user.name} used {this.name}, dealing {modifiedDamage} {this.element.ToString()} damage to {target.name}.");
        }
    }
}
