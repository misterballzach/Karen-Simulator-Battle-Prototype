using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition { get; private set; }
    public bool isOccupied { get; set; } = false;

    private GridManager gridManager;

    public void Init(Vector2Int position, GridManager manager)
    {
        gridPosition = position;
        gridManager = manager;
    }

    // Additional methods for highlighting, etc. can be added here later
}
