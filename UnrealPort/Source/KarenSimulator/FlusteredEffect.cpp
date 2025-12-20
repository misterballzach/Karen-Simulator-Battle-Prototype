#include "FlusteredEffect.h"
#include "Combatant.h"
#include "Math/UnrealMathUtility.h"

UFlusteredEffect::UFlusteredEffect()
{
    Name = "Flustered";
    Description = "This unit is flustered and has a chance to skip their turn.";
    Type = EStatusEffectType::Debuff;
    MissChance = 0.5f;
}

bool UFlusteredEffect::CheckIfTurnSkipped()
{
    bool bSkipped = FMath::FRand() < MissChance;
    if (bSkipped && Owner)
    {
        UE_LOG(LogTemp, Log, TEXT("%s is flustered and skipped their turn!"), *Owner->GetName());
    }
    return bSkipped;
}
