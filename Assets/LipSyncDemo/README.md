# uLipSync Demo Setup

This document explains how to set up the `uLipSync` demonstration scene in this project.

## Prerequisites

- The `uLipSync` plugin has been installed in `Assets/Plugins/uLipSync`.
- The sample audio file has been moved to `Assets/LipSyncDemo/Audio`.
- The sample phoneme textures have been copied to `Assets/LipSyncDemo/Textures`.

## Setup Steps

1.  **Create a New Scene:**
    - In the Unity Editor, go to `File > New Scene`.
    - Save the scene in the `Assets/LipSyncDemo` folder (e.g., as `LipSyncDemoScene`).

2.  **Create a Quad Object:**
    - In the Hierarchy window, right-click and select `3D Object > Quad`.
    - This will create a simple 2D plane in the scene.

3.  **Attach the Demo Script:**
    - Select the `Quad` object in the Hierarchy.
    - In the Inspector window, click `Add Component` and search for `LipSyncDemo`.
    - Add the `LipSyncDemo` script to the Quad.

4.  **Configure the Demo Script:**
    - With the `Quad` still selected, look at the `LipSyncDemo` component in the Inspector.
    - You will see several empty fields. You need to assign the assets to them.

    - **Profile:**
        - Click the circle icon next to the `Profile` field.
        - In the window that appears, search for `uLipSync-Profile-Sample` and select it. This is a generic profile that comes with `uLipSync`.

    - **Audio Clip:**
        - Drag the audio file from `Assets/LipSyncDemo/Audio` into the `Audio Clip` field.

    - **Phoneme Textures:**
        - Drag the corresponding texture from `Assets/LipSyncDemo/Textures` to each texture slot (`A`, `I`, `U`, `E`, `O`, `N`, `Default`).
        - For `A`, use `hecomi-a.png`.
        - For `I`, use `hecomi-i.png`.
        - For `U`, use `hecomi-u.png`.
        - For `E`, use `hecomi-e.png`.
        - For `O`, use `hecomi-o.png`.
        - For `N`, use `hecomi-n.png`.
        - For `Default`, use `hecomi-base.png`.

5.  **Position the Camera:**
    - Make sure the `Main Camera` in the scene is positioned to see the `Quad`. You may need to move it or the Quad.

6.  **Run the Scene:**
    - Press the Play button at the top of the Unity Editor.
    - You should now hear the audio playing and see the texture on the Quad changing in sync with the voice.

This completes the setup of the `uLipSync` demonstration. You can use the `LipSyncDemo.cs` script as a reference for how to use `uLipSync` in your own game scripts.
