#include "EntitledRageAbility.h"
#include "Combatant.h"

void UEntitledRageAbility::Use_Implementation(ACombatant* User, ACombatant* Target)
{
    if (Target)
    {
        UE_LOG(LogTemp, Log, TEXT("%s uses Entitled Rage!"), *User->GetName());
        User->AnimateAttack(Target);
        Target->TakeEmotionalDamage(Damage, RhetoricalClass, User);
        User->GainEntitlement(15);
    }
}
