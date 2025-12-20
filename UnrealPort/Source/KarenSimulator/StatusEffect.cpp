#include "StatusEffect.h"
#include "Combatant.h"

UStatusEffect::UStatusEffect()
{
    Duration = 0;
    Type = EStatusEffectType::Debuff;
}

void UStatusEffect::Apply(ACombatant* Target)
{
    Owner = Target;
}

void UStatusEffect::OnTurnStart_Implementation()
{
    // Logic to be executed at the start of the turn
}

void UStatusEffect::OnTurnEnd_Implementation()
{
    Duration--;
}

void UStatusEffect::Remove_Implementation()
{
    // Logic to be executed when the effect is removed
}
