#pragma once

#include "CoreMinimal.h"
#include "Engine/GameInstance.h"
#include "KarenEnums.h"
#include "KarenGameInstance.generated.h"

UCLASS()
class KARENSIMULATOR_API UKarenGameInstance : public UGameInstance
{
    GENERATED_BODY()

public:
    virtual void Init() override;

    UPROPERTY(BlueprintReadOnly, Category = "Game State")
    EKarenGameState CurrentState;

    UFUNCTION(BlueprintCallable, Category = "Game State")
    void UpdateGameState(EKarenGameState NewState);

    UFUNCTION(BlueprintCallable, Category = "Game State")
    void StartBattle();

    UFUNCTION(BlueprintCallable, Category = "Game State")
    void EndBattle(bool bPlayerWon);
};
