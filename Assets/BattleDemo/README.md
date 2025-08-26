# Battle Demo Instructions

This folder contains a procedural battle demo. Follow the steps below to run it.

## How to Run the Demo

1.  **Open the project** in the Unity Editor.

2.  **Create a new scene:**
    - Go to `File > New Scene`.
    - Choose "Basic (built-in)" or "Empty" and click "Create".

3.  **Create a generator object:**
    - In the Hierarchy window, right-click and select "Create Empty".
    - Name the new GameObject "DemoGenerator".

4.  **Attach the generator script:**
    - Select the "DemoGenerator" object.
    - In the Inspector window, click "Add Component".
    - Search for and add the `BattleDemoGenerator` script.

5.  **Assign the sprite asset:**
    - The `BattleDemoGenerator` component has a field called "Combatant Sprite".
    - In the Project window, navigate to the `Assets/BattleDemo` folder.
    - Find the `hecomi-base` sprite.
    - **Drag** the `hecomi-base` sprite from the Project window into the "Combatant Sprite" field in the Inspector. This field is on the "DemoGenerator" object you created.

6.  **Play the scene:**
    - Press the Play button at the top of the Unity Editor.

The battle will now generate and start automatically. You will see a player and an enemy, and a hand of cards at the bottom of the screen. Click a card to use it, and press "End Turn" to let the enemy take its turn.
