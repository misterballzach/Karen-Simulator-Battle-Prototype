using UnityEngine;

[CreateAssetMenu(fileName = "Trauma Dump", menuName = "KAREN/Abilities/Trauma Dump")]
public class TraumaDumpAbility : VerbalAbility
{
    public int argumentsToPrepare;

    public void Use(Combatant user, Combatant target)
    {
        if (user != null)
        {
            for (int i = 0; i < argumentsToPrepare; i++)
            {
                user.PrepareArgument();
            }
            Debug.Log($"{user.name} used {this.name} to prepare {argumentsToPrepare} new arguments.");
        }
    }
}
