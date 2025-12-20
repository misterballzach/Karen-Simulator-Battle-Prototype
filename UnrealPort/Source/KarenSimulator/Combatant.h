#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "KarenEnums.h"
#include "VerbalAbility.h"
#include "StatusEffect.h"
#include "Combatant.generated.h"

UCLASS()
class KARENSIMULATOR_API ACombatant : public AActor
{
    GENERATED_BODY()

public:
    ACombatant();

protected:
    virtual void BeginPlay() override;

public:
    virtual void Tick(float DeltaTime) override;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Identity")
    EFaction Faction;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Identity")
    EKarenClass KarenClass;

    // Sprite handling would typically use a Paper2D component or a widget component in 3D
    // Leaving as a placeholder UObject for now or omitting.

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 Level = 1;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 ExperiencePoints = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 XpToNextLevel = 100;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 MaxEmotionalStamina = 100;

    UPROPERTY(BlueprintReadOnly, Category = "Stats")
    int32 CurrentEmotionalStamina;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 MaxCredibility = 100;

    UPROPERTY(BlueprintReadOnly, Category = "Stats")
    int32 CurrentCredibility;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Stats")
    int32 Armor = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Dual-Axis Meters")
    int32 Entitlement = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Dual-Axis Meters")
    int32 MaxEntitlement = 100;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Dual-Axis Meters")
    int32 Insight = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Dual-Axis Meters")
    int32 MaxInsight = 100;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Critical Hits")
    float CritChance = 0.05f;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Critical Hits")
    float CritDamageMultiplier = 1.5f;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Modifiers")
    float OutgoingDamageModifier = 1.0f;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Modifiers")
    float IncomingDamageModifier = 1.0f;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Modifiers")
    float IncomingHealingModifier = 1.0f;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Modifiers")
    int32 CredibilityCostModifier = 0;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Loadout")
    TArray<UVerbalAbility*> VerbalLoadout;

    UPROPERTY(BlueprintReadOnly, Category = "Loadout")
    TArray<UVerbalAbility*> PreparedArguments;

    UPROPERTY(BlueprintReadOnly, Category = "Loadout")
    TArray<UVerbalAbility*> UsedArguments;

    UPROPERTY(BlueprintReadOnly, Category = "Status Effects")
    TArray<UStatusEffect*> StatusEffects;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Resistances")
    TArray<ERhetoricalClass> RhetoricalWeaknesses;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Resistances")
    TArray<ERhetoricalClass> RhetoricalResistances;

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void TakeEmotionalDamage(int32 Amount, ERhetoricalClass RhetoricalType, ACombatant* Attacker);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void RecoverStamina(int32 Amount);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void PrepareArgument();

    UFUNCTION(BlueprintCallable, Category = "Combat")
    bool UseAbility(UVerbalAbility* Ability, ACombatant* Target);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void ShuffleLoadout();

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void AddStatusEffect(UStatusEffect* Effect);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    bool CanTakeTurn();

    UFUNCTION(BlueprintCallable, Category = "Turn")
    void OnTurnStart();

    UFUNCTION(BlueprintCallable, Category = "Turn")
    void OnTurnEnd();

    UFUNCTION(BlueprintCallable, Category = "Progression")
    void GainXP(int32 Amount);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void GainEntitlement(int32 Amount);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void GainInsight(int32 Amount);

    UFUNCTION(BlueprintCallable, Category = "Combat")
    void AnimateAttack(ACombatant* Target);

private:
    void LevelUp();
    void ApplyClassBonuses();
};
