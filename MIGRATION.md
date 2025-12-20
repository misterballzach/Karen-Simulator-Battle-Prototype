# Migration Guide: Unity to Unreal Engine

This directory contains the C++ port of the core gameplay logic from the Unity project "Karen Simulator".

## Structure

The ported code is located in `UnrealPort/Source/KarenSimulator`.

*   **KarenSimulator.h/cpp**: The main game module.
*   **KarenEnums.h**: Contains core enums (`EKarenClass`, `EFaction`, `ERhetoricalClass`, `EKarenGameState`).
*   **Combatant.h/cpp**: The core character class (`ACombatant`), ported from `Combatant.cs`. Inherits from `AActor`.
*   **VerbalAbility.h/cpp**: The base class for abilities (`UVerbalAbility`), ported from `VerbalAbility.cs`. Inherits from `UDataAsset`.
*   **StatusEffect.h/cpp**: The base class for effects (`UStatusEffect`), ported from `StatusEffect.cs`. Inherits from `UObject`.
*   **KarenGameInstance.h/cpp**: Manages global game state, ported from `GameManager.cs`.

### Representative Subclasses

A few subclasses have been ported to demonstrate the pattern:
*   `EntitledRageAbility` (Ability)
*   `SharedTraumaAbility` (Ability)
*   `FlusteredEffect` (Status Effect)

## Integration Steps

1.  **Create a New Unreal Project**:
    *   Open Unreal Engine 5.
    *   Create a new C++ project named `KarenSimulator`.
    *   Close the Unreal Editor.

2.  **Copy Source Files**:
    *   Copy the contents of `UnrealPort/Source/KarenSimulator` into your new project's `Source/KarenSimulator` directory, overwriting existing files if prompted (be careful to keep the generated target files).

3.  **Regenerate Project Files**:
    *   Right-click the `.uproject` file and select "Generate Visual Studio project files".

4.  **Compile**:
    *   Open the solution in Visual Studio (or Rider/VSCode) and build the project.

5.  **Blueprint Setup**:
    *   Open the Unreal Editor.
    *   Create a Blueprint class inheriting from `ACombatant` for your characters.
    *   Create Data Assets derived from `UVerbalAbility` (or `UEntitledRageAbility`, etc.) to define your abilities.
    *   Assign these Data Assets to the `VerbalLoadout` of your Combatant Blueprints.

## Not Ported / TODO

*   **UI**: Unity UI (Canvas, RectTransform) is completely different from Unreal UMG. You will need to recreate the UI using Widget Blueprints in Unreal. The C++ classes expose data (properties marked `BlueprintReadWrite`) that your Widgets can bind to.
*   **Assets**: Unity `.meta` files, scenes (`.unity`), and prefabs (`.prefab`) are not compatible. You must re-import textures, audio, and models manually.
*   **Specific Subclasses**: Only a few representative abilities and effects were ported. You will need to port the remaining scripts following the patterns established in `EntitledRageAbility.h/cpp` and `FlusteredEffect.h/cpp`.
*   **Audio**: The `AudioManager` stub is not implemented. Use Unreal's `USoundCue` and `UAudioComponent` systems.

## Key Concept Changes

*   `MonoBehaviour` -> `AActor` (for world objects) or `UActorComponent`.
*   `ScriptableObject` -> `UDataAsset`.
*   `Instantiate()` -> `NewObject<T>()` or `GetWorld()->SpawnActor<T>()`.
*   `Debug.Log` -> `UE_LOG`.
*   `List<T>` -> `TArray<T>`.
