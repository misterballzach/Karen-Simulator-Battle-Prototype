using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building Settings")]
    public string buildingName;
    public int cost;
    public Vector2Int size = Vector2Int.one;

    // This will store the grid position of the building's origin tile
    public Vector2Int gridPosition;

    public void Place(Vector2Int position)
    {
        gridPosition = position;
        // Additional placement logic can be added here
    }
}
