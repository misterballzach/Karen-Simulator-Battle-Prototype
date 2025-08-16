using UnityEngine;

[CreateAssetMenu(fileName = "Fake Cry", menuName = "KAREN/Abilities/Fake Cry")]
public class FakeCryAbility : VerbalAbility
{
    public override void Use(Combatant user, Combatant target)
    {
        // This ability should target the user
        if (user != null)
        {
            user.RecoverStamina(healing);
            Debug.Log($"{user.name} used {this.name} to recover {healing} emotional stamina.");
        }
    }
}
