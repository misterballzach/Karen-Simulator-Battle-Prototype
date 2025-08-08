using UnityEngine;

public class CommuneUI : MonoBehaviour
{
    public GameObject workshopPrefab; // Assign this in the inspector

    private void OnGUI()
    {
        // --- Resource Display ---
        // A simple box at the top-left of the screen to display resources.
        GUI.Box(new Rect(10, 10, 200, 50), "Commune Resources");
        if (CommuneManager.s_instance != null)
        {
            GUI.Label(new Rect(20, 30, 180, 20), $"Insight: {CommuneManager.s_instance.insightResource}");
            // GUI.Label(new Rect(20, 50, 180, 20), $"Trust: {CommuneManager.s_instance.communityTrust}");
        }

        // --- Build Menu ---
        // A simple button to start placing a workshop.
        // This would be more complex in a real game with multiple buildings.
        GUI.Box(new Rect(10, 70, 200, 60), "Build Menu");
        if (GUI.Button(new Rect(20, 90, 180, 30), "Build Workshop"))
        {
            if (BuildingPlacer.s_instance != null && workshopPrefab != null)
            {
                BuildingPlacer.s_instance.StartPlacingBuilding(workshopPrefab);
            }
            else
            {
                Debug.LogError("BuildingPlacer instance or workshopPrefab not set!");
            }
        }
    }
}
