using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect Card", menuName = "Cards/Status Effect Card")]
public class StatusEffectCard : Card
{
    [SerializeReference]
    public StatusEffect effectToApply;

    public override void Use(Entity user, Entity target)
    {
        if (target != null && effectToApply != null)
        {
            // Create a new instance of the effect to ensure it's unique
            StatusEffect newEffectInstance = System.Activator.CreateInstance(effectToApply.GetType()) as StatusEffect;
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(effectToApply), newEffectInstance);

            target.AddStatusEffect(newEffectInstance);
            Debug.Log($"{this.name} applied {newEffectInstance.name} to {target.name}.");
        }
    }
}
