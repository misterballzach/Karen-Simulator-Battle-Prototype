#pragma once

#include "CoreMinimal.h"
#include "StatusEffect.h"
#include "FlusteredEffect.generated.h"

UCLASS()
class KARENSIMULATOR_API UFlusteredEffect : public UStatusEffect
{
    GENERATED_BODY()

public:
    UFlusteredEffect();

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect", meta = (ClampMin = "0.0", ClampMax = "1.0"))
    float MissChance;

    UFUNCTION(BlueprintCallable, Category = "Status Effect")
    bool CheckIfTurnSkipped();
};
