using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager s_instance;

    [Header("Grid Settings")]
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float tileSize = 1.0f;
    public GameObject tilePrefab;

    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile Prefab is not assigned in the GridManager.");
            return;
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Isometric projection
                Vector3 tilePos = new Vector3(x * 0.5f + y * 0.5f, x * -0.25f + y * 0.25f, 0) * tileSize;
                GameObject newTileObj = Instantiate(tilePrefab, tilePos, Quaternion.identity, this.transform);
                newTileObj.name = $"Tile_{x}_{y}";

                Tile newTile = newTileObj.AddComponent<Tile>();
                Vector2Int gridPos = new Vector2Int(x, y);
                newTile.Init(gridPos, this);

                tiles.Add(gridPos, newTile);
            }
        }
    }

    public Tile GetTile(Vector2Int gridPos)
    {
        if (tiles.ContainsKey(gridPos))
        {
            return tiles[gridPos];
        }
        return null;
    }
}
