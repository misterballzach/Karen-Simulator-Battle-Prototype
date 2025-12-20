#include "SharedTraumaAbility.h"
#include "Combatant.h"
#include "Math/UnrealMathUtility.h"

void USharedTraumaAbility::Use_Implementation(ACombatant* User, ACombatant* Target)
{
    UE_LOG(LogTemp, Log, TEXT("%s shares trauma."), *User->GetName());
    User->AnimateAttack(Target);

    // User takes half damage
    int32 SelfDamage = FMath::RoundToInt(Damage * 0.5f);
    User->TakeEmotionalDamage(SelfDamage, RhetoricalClass, nullptr);

    Target->TakeEmotionalDamage(Damage, RhetoricalClass, User);

    User->GainInsight(10);
    Target->GainInsight(10);
}
