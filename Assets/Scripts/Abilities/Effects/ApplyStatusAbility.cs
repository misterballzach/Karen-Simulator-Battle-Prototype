using UnityEngine;

[CreateAssetMenu(fileName = "Apply Status", menuName = "KAREN/Abilities/Apply Status")]
public class ApplyStatusAbility : VerbalAbility
{
    [SerializeReference]
    public StatusEffect effectToApply;

    public void Use(Combatant user, Combatant target)
    {
        if (target != null && effectToApply != null)
        {
            StatusEffect newEffectInstance = (StatusEffect)System.Activator.CreateInstance(effectToApply.GetType());
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(effectToApply), newEffectInstance);

            target.AddStatusEffect(newEffectInstance);
            Debug.Log($"{user.name} used {this.name}, applying {newEffectInstance.name} to {target.name}.");
        }
    }
}
