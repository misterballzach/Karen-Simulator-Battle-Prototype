using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Encounter Event", menuName = "KAREN/Mindscape/Encounter Event")]
public class EncounterEvent : MindscapeEvent
{
    // In a real game, this might hold enemy data, location data, etc.
    // Or it could hold a reference to a scene to load.
    public string encounterSceneName;

    public override void Trigger()
    {
        Debug.Log($"Triggering encounter event, loading scene: {encounterSceneName}");
        // A real implementation would have a system to pass data to the loaded scene.
        SceneManager.LoadScene(encounterSceneName);
    }
}
