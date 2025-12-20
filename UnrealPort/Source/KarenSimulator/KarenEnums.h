#pragma once

#include "CoreMinimal.h"
#include "KarenEnums.generated.h"

UENUM(BlueprintType)
enum class EKarenClass : uint8
{
    None            UMETA(DisplayName = "None"),
    ClassicKaren    UMETA(DisplayName = "Classic Karen"),
    WellnessWitch   UMETA(DisplayName = "Wellness Witch"),
    KarenNouveau    UMETA(DisplayName = "Karen Nouveau"),
    RepentantKaren  UMETA(DisplayName = "Repentant Karen")
};

UENUM(BlueprintType)
enum class EFaction : uint8
{
    None                UMETA(DisplayName = "None"),
    Player              UMETA(DisplayName = "Player"),
    Enemy               UMETA(DisplayName = "Enemy"),
    Boomers             UMETA(DisplayName = "Boomers"),
    RetailWorkers       UMETA(DisplayName = "Retail Workers"),
    HOACouncil          UMETA(DisplayName = "HOA Council"),
    OnlineCommunities   UMETA(DisplayName = "Online Communities"),
    MillennialMoms      UMETA(DisplayName = "Millennial Moms"),
    GenZRenters         UMETA(DisplayName = "Gen Z Renters")
};

UENUM(BlueprintType)
enum class ERhetoricalClass : uint8
{
    Neutral         UMETA(DisplayName = "Neutral"),
    Aggression      UMETA(DisplayName = "Aggression"),
    Manipulation    UMETA(DisplayName = "Manipulation"),
    Delusion        UMETA(DisplayName = "Delusion"),
    Vulnerability   UMETA(DisplayName = "Vulnerability")
};

UENUM(BlueprintType)
enum class EKarenGameState : uint8
{
    MainMenu    UMETA(DisplayName = "Main Menu"),
    InBattle    UMETA(DisplayName = "In Battle"),
    BattleWon   UMETA(DisplayName = "Battle Won"),
    BattleLost  UMETA(DisplayName = "Battle Lost")
};
