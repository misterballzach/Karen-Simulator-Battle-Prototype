#pragma once

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "KarenEnums.h"
#include "StatusEffect.generated.h"

class ACombatant;

UENUM(BlueprintType)
enum class EStatusEffectType : uint8
{
    Buff,
    Debuff
};

// Interface for on damage triggers
UINTERFACE(MinimalAPI)
class UOnDamageTrigger : public UInterface
{
    GENERATED_BODY()
};

class IOnDamageTrigger
{
    GENERATED_BODY()

public:
    virtual bool OnTakeDamage(ACombatant* Attacker, int32& Damage) = 0;
};

UCLASS(Blueprintable, BlueprintType)
class KARENSIMULATOR_API UStatusEffect : public UObject
{
    GENERATED_BODY()

public:
    UStatusEffect();

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect")
    FString Name;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect", meta = (MultiLine = "true"))
    FString Description;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect")
    int32 Duration;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect")
    EStatusEffectType Type;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Status Effect")
    TArray<ERhetoricalClass> BlockedRhetoric;

    UPROPERTY(BlueprintReadOnly, Category = "Status Effect")
    ACombatant* Owner;

    virtual void Apply(ACombatant* Target);

    UFUNCTION(BlueprintNativeEvent, Category = "Status Effect")
    void OnTurnStart();
    virtual void OnTurnStart_Implementation();

    UFUNCTION(BlueprintNativeEvent, Category = "Status Effect")
    void OnTurnEnd();
    virtual void OnTurnEnd_Implementation();

    UFUNCTION(BlueprintNativeEvent, Category = "Status Effect")
    void Remove();
    virtual void Remove_Implementation();
};
