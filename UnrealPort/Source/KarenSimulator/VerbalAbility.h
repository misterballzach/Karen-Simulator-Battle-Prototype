#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "KarenEnums.h"
#include "VerbalAbility.generated.h"

class ACombatant;

UCLASS(Blueprintable, BlueprintType)
class KARENSIMULATOR_API UVerbalAbility : public UDataAsset
{
    GENERATED_BODY()

public:
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Display Info")
    FString AbilityName; // "Name" is reserved in UObject

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Display Info", meta = (MultiLine = "true"))
    FString Description;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Display Info")
    UTexture2D* Artwork;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    int32 Cost;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    int32 Damage = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    int32 Healing = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    ERhetoricalClass RhetoricalClass;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    bool bIsDebuff;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay")
    int32 Cooldown;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Gameplay", meta = (ClampMin = "0.0", ClampMax = "1.0"))
    float EscalationRisk;

    // Skipping ReputationModifiers and AbilityUpgrade for brevity/stubbing

    UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "Ability")
    void Use(ACombatant* User, ACombatant* Target);
    virtual void Use_Implementation(ACombatant* User, ACombatant* Target);
};
