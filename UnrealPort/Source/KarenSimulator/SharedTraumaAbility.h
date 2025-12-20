#pragma once

#include "CoreMinimal.h"
#include "VerbalAbility.h"
#include "SharedTraumaAbility.generated.h"

UCLASS()
class KARENSIMULATOR_API USharedTraumaAbility : public UVerbalAbility
{
    GENERATED_BODY()

public:
    virtual void Use_Implementation(ACombatant* User, ACombatant* Target) override;
};
