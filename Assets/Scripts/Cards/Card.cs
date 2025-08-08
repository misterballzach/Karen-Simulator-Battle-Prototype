using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite artwork;
    public int cost;
    public Element element;

    public virtual void Use(Entity user, Entity target)
    {
        // This method will be overridden by specific card types
    }
}
