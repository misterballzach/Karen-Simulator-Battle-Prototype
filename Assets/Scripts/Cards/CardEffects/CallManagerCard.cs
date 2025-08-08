using UnityEngine;

[CreateAssetMenu(fileName = "Call the Manager", menuName = "Cards/Call The Manager")]
public class CallManagerCard : Card
{
    public CallManagerCard()
    {
        name = "Call the Manager";
        cost = 0; // No mana cost, only escalation
    }

    public override void Use(Entity user, Entity target)
    {
        if (target != null)
        {
            Debug.Log($"{user.name} calls the manager on {target.name}!");
            target.AddStatusEffect(new FiredEffect());
        }
    }
}
