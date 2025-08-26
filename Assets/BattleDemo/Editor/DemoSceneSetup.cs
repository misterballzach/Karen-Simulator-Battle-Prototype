using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class DemoSceneSetup
{
    private const string DEMO_SCENE_PATH = "Assets/BattleDemo/BattleDemoScene.unity";
    private const string SPRITE_PATH = "Assets/BattleDemo/character_sprite.png";

    [MenuItem("Karen Simulator/Setup Battle Demo Scene")]
    public static void SetupScene()
    {
        // 1. Create a new scene
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // 2. Create the generator object
        GameObject generatorGO = new GameObject("DemoGenerator");

        // 3. Attach the generator script
        BattleDemoGenerator generatorComponent = generatorGO.AddComponent<BattleDemoGenerator>();

        // Ensure the asset is imported as a Sprite
        TextureImporter textureImporter = AssetImporter.GetAtPath(SPRITE_PATH) as TextureImporter;
        if (textureImporter != null)
        {
            if (textureImporter.textureType != TextureImporterType.Sprite)
            {
                Debug.Log("Correcting texture type for " + SPRITE_PATH + ". Setting to 'Sprite'.");
                textureImporter.textureType = TextureImporterType.Sprite;
                AssetDatabase.ImportAsset(SPRITE_PATH, ImportAssetOptions.ForceUpdate);
            }
        }
        else
        {
            Debug.LogError("Could not get TextureImporter for path: " + SPRITE_PATH + ". The file might be missing or not a texture.");
            return;
        }

        // 4. Load the sprite asset
        Sprite combatantSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SPRITE_PATH);

        if (combatantSprite == null)
        {
            Debug.LogError("Could not find combatant sprite at path: " + SPRITE_PATH + ". Please ensure the asset exists and is imported as a Sprite.");
            return;
        }

        // 5. Assign the sprite
        generatorComponent.combatantSprite = combatantSprite;

        // Mark the object as dirty to ensure the change is saved
        EditorUtility.SetDirty(generatorComponent);

        Debug.Log("Battle Demo Generator object created and configured.");

        // 6. Save the new scene
        bool saveOK = EditorSceneManager.SaveScene(newScene, DEMO_SCENE_PATH);
        if (saveOK)
        {
            Debug.Log("Successfully created and saved the battle demo scene at: " + DEMO_SCENE_PATH);
            EditorUtility.DisplayDialog(
                "Scene Setup Complete",
                "The Battle Demo Scene has been created successfully.\n\nYou can now open '" + DEMO_SCENE_PATH + "' and press Play.",
                "OK");
        }
        else
        {
            Debug.LogError("Failed to save the scene at " + DEMO_SCENE_PATH);
        }
    }
}
