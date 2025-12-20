#pragma once

#include "CoreMinimal.h"
#include "VerbalAbility.h"
#include "EntitledRageAbility.generated.h"

UCLASS()
class KARENSIMULATOR_API UEntitledRageAbility : public UVerbalAbility
{
    GENERATED_BODY()

public:
    virtual void Use_Implementation(ACombatant* User, ACombatant* Target) override;
};
