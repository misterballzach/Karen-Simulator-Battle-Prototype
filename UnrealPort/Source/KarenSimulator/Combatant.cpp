#include "Combatant.h"
#include "Kismet/GameplayStatics.h"
#include "Math/UnrealMathUtility.h"

ACombatant::ACombatant()
{
    PrimaryActorTick.bCanEverTick = true;
}

void ACombatant::BeginPlay()
{
    Super::BeginPlay();
    CurrentEmotionalStamina = MaxEmotionalStamina;
    CurrentCredibility = MaxCredibility;
    ShuffleLoadout();
}

void ACombatant::Tick(float DeltaTime)
{
    Super::Tick(DeltaTime);
}

void ACombatant::TakeEmotionalDamage(int32 Amount, ERhetoricalClass RhetoricalType, ACombatant* Attacker)
{
    int32 ModifiedDamage = Amount;

    // Pre-damage triggers
    for (UStatusEffect* Effect : StatusEffects)
    {
        // Check if implemented interface
        if (Effect->Implements<UOnDamageTrigger>())
        {
            IOnDamageTrigger* Trigger = Cast<IOnDamageTrigger>(Effect);
            if (Trigger && !Trigger->OnTakeDamage(Attacker, ModifiedDamage))
            {
                UE_LOG(LogTemp, Log, TEXT("Damage cancelled by %s"), *Effect->Name);
                return;
            }
        }
    }

    bool bIsCrit = false;
    if (Attacker && FMath::FRand() < Attacker->CritChance)
    {
        bIsCrit = true;
        UE_LOG(LogTemp, Log, TEXT("Critical Hit!"));
    }

    float CritMultiplier = bIsCrit ? Attacker->CritDamageMultiplier : 1.0f;
    float RhetoricalMultiplier = 1.0f;

    if (RhetoricalWeaknesses.Contains(RhetoricalType))
    {
        RhetoricalMultiplier = 2.0f;
    }
    else if (RhetoricalResistances.Contains(RhetoricalType))
    {
        RhetoricalMultiplier = 0.5f;
    }

    int32 TotalDamage = FMath::RoundToInt(ModifiedDamage * CritMultiplier * RhetoricalMultiplier * IncomingDamageModifier);
    int32 DamageToArmor = FMath::Min(Armor, TotalDamage);
    Armor -= DamageToArmor;
    int32 DamageToHealth = TotalDamage - DamageToArmor;

    CurrentEmotionalStamina -= DamageToHealth;
    if (CurrentEmotionalStamina < 0) CurrentEmotionalStamina = 0;

    UE_LOG(LogTemp, Log, TEXT("%s took %d emotional damage (%d absorbed by armor)."), *GetName(), DamageToHealth, DamageToArmor);

    // Audio hook placeholder
    // if (AudioManager) AudioManager->PlayTakeDamageSound();
}

void ACombatant::RecoverStamina(int32 Amount)
{
    int32 TotalHealing = FMath::RoundToInt(Amount * IncomingHealingModifier);
    CurrentEmotionalStamina += TotalHealing;
    if (CurrentEmotionalStamina > MaxEmotionalStamina) CurrentEmotionalStamina = MaxEmotionalStamina;
    UE_LOG(LogTemp, Log, TEXT("%s recovered %d stamina."), *GetName(), TotalHealing);
}

void ACombatant::PrepareArgument()
{
    if (VerbalLoadout.Num() == 0)
    {
        for (UVerbalAbility* Ability : UsedArguments)
        {
            VerbalLoadout.Add(Ability);
        }
        UsedArguments.Empty();
        ShuffleLoadout();
    }

    if (VerbalLoadout.Num() > 0)
    {
        UVerbalAbility* Ability = VerbalLoadout[0];
        VerbalLoadout.RemoveAt(0);
        PreparedArguments.Add(Ability);
    }
}

bool ACombatant::UseAbility(UVerbalAbility* Ability, ACombatant* Target)
{
    if (!PreparedArguments.Contains(Ability)) return false;

    // Check status effects
    for (UStatusEffect* Effect : StatusEffects)
    {
        if (Effect->BlockedRhetoric.Contains(Ability->RhetoricalClass))
        {
            UE_LOG(LogTemp, Warning, TEXT("Cannot use ability due to %s"), *Effect->Name);
            return false;
        }
    }

    // Cost logic
    int32 FinalCost = FMath::Max(0, Ability->Cost + CredibilityCostModifier);
    if (CurrentCredibility < FinalCost)
    {
        return false;
    }

    CurrentCredibility -= FinalCost;

    Ability->Use(this, Target);

    PreparedArguments.RemoveSingle(Ability);
    UsedArguments.Add(Ability);

    return true;
}

void ACombatant::ShuffleLoadout()
{
    // Fisher-Yates shuffle
    const int32 Num = VerbalLoadout.Num();
    for (int32 i = 0; i < Num; ++i)
    {
        int32 j = FMath::RandRange(i, Num - 1);
        VerbalLoadout.Swap(i, j);
    }
}

void ACombatant::AddStatusEffect(UStatusEffect* Effect)
{
    StatusEffects.Add(Effect);
    Effect->Apply(this);
}

bool ACombatant::CanTakeTurn()
{
    // Need to cast to specific effect types or check tags
    // Placeholder logic for now, as specific FlusteredEffect checking requires casting
    return true;
}

void ACombatant::OnTurnStart()
{
    CurrentCredibility += 10;
    if (CurrentCredibility > MaxCredibility) CurrentCredibility = MaxCredibility;

    for (UStatusEffect* Effect : StatusEffects)
    {
        Effect->OnTurnStart();
    }
}

void ACombatant::OnTurnEnd()
{
    if (Armor > 0) Armor = FMath::FloorToInt(Armor * 0.5f);

    TArray<UStatusEffect*> EffectsToRemove;
    for (UStatusEffect* Effect : StatusEffects)
    {
        Effect->OnTurnEnd();
        if (Effect->Duration <= 0) EffectsToRemove.Add(Effect);
    }

    for (UStatusEffect* Effect : EffectsToRemove)
    {
        Effect->Remove();
        StatusEffects.Remove(Effect);
    }
}

void ACombatant::GainXP(int32 Amount)
{
    ExperiencePoints += Amount;
    if (ExperiencePoints >= XpToNextLevel)
    {
        LevelUp();
    }
}

void ACombatant::LevelUp()
{
    Level++;
    ExperiencePoints -= XpToNextLevel;
    XpToNextLevel = FMath::RoundToInt(XpToNextLevel * 1.5f);

    MaxEmotionalStamina += 10;
    MaxCredibility += 5;
    ApplyClassBonuses();

    CurrentEmotionalStamina = MaxEmotionalStamina;
    CurrentCredibility = MaxCredibility;
}

void ACombatant::ApplyClassBonuses()
{
    switch (KarenClass)
    {
        case EKarenClass::ClassicKaren:
            MaxEntitlement -= 5;
            break;
        case EKarenClass::WellnessWitch:
            IncomingHealingModifier += 0.1f;
            break;
        default:
            break;
    }
}

void ACombatant::GainEntitlement(int32 Amount)
{
    Entitlement += Amount;
    if (Entitlement >= MaxEntitlement)
    {
        Entitlement = 0;
        // Trigger Code Blonde (needs implementation)
    }
}

void ACombatant::GainInsight(int32 Amount)
{
    Insight += Amount;
    if (Insight >= MaxInsight)
    {
        // Win condition logic
    }
}

void ACombatant::AnimateAttack(ACombatant* Target)
{
    UE_LOG(LogTemp, Log, TEXT("%s attacks %s"), *GetName(), *Target->GetName());
}
