#include "KarenGameInstance.h"

void UKarenGameInstance::Init()
{
    Super::Init();
    UpdateGameState(EKarenGameState::MainMenu);
}

void UKarenGameInstance::UpdateGameState(EKarenGameState NewState)
{
    CurrentState = NewState;

    switch (NewState)
    {
    case EKarenGameState::MainMenu:
        UE_LOG(LogTemp, Log, TEXT("Game State: Main Menu"));
        break;
    case EKarenGameState::InBattle:
        UE_LOG(LogTemp, Log, TEXT("Game State: In Battle"));
        break;
    case EKarenGameState::BattleWon:
        UE_LOG(LogTemp, Log, TEXT("Game State: Battle Won"));
        break;
    case EKarenGameState::BattleLost:
        UE_LOG(LogTemp, Log, TEXT("Game State: Battle Lost"));
        break;
    }
}

void UKarenGameInstance::StartBattle()
{
    UpdateGameState(EKarenGameState::InBattle);
    // OpenLevel logic would go here
}

void UKarenGameInstance::EndBattle(bool bPlayerWon)
{
    if (bPlayerWon)
    {
        UpdateGameState(EKarenGameState::BattleWon);
    }
    else
    {
        UpdateGameState(EKarenGameState::BattleLost);
    }
}
