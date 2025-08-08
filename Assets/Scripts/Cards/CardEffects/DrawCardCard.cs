using UnityEngine;

[CreateAssetMenu(fileName = "New Draw Card Card", menuName = "Cards/Draw Card Card")]
public class DrawCardCard : Card
{
    public int cardsToDraw;

    public override void Use(Entity user, Entity target)
    {
        if (user != null)
        {
            for (int i = 0; i < cardsToDraw; i++)
            {
                user.DrawCard();
            }
            Debug.Log($"{this.name} made {user.name} draw {cardsToDraw} cards.");
        }
    }
}
