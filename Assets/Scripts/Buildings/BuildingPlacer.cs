using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer s_instance;

    public GameObject buildingPrefab; // Assign the Workshop prefab in the inspector
    private Building currentBuilding;
    private GameObject buildingPreview;

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

    void Update()
    {
        if (currentBuilding == null)
        {
            // If we are not in build mode, do nothing.
            // The CommuneUI script now handles starting the build process.
            return;
        }

        Vector2Int gridPos = GetMouseGridPosition();
        if (gridPos != new Vector2Int(-1, -1))
        {
            buildingPreview.transform.position = GridManager.s_instance.GetTile(gridPos).transform.position;
            // Here you would add visual feedback, e.g., changing color if the location is invalid.

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(gridPos);
            }
        }
    }

    public void StartPlacingBuilding(GameObject prefab)
    {
        if (prefab == null) return;

        currentBuilding = prefab.GetComponent<Building>();
        if (currentBuilding == null)
        {
            Debug.LogError("Prefab does not have a Building component.");
            return;
        }

        buildingPreview = Instantiate(prefab);
        // Disable any functional components on the preview, it's just for show
        foreach (var component in buildingPreview.GetComponents<MonoBehaviour>())
        {
            if (!(component is Building))
            {
                component.enabled = false;
            }
        }
    }

    private Vector2Int GetMouseGridPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Tile tile = hitInfo.collider.GetComponent<Tile>();
            if (tile != null)
            {
                return tile.gridPosition;
            }
        }
        return new Vector2Int(-1, -1); // Return invalid position if no tile is hit
    }

    private void PlaceBuilding(Vector2Int gridPos)
    {
        Tile targetTile = GridManager.s_instance.GetTile(gridPos);
        if (targetTile == null || targetTile.isOccupied)
        {
            Debug.Log("Cannot place building here. Tile is occupied or invalid.");
            return;
        }

        if (CommuneManager.s_instance.insightResource < currentBuilding.cost)
        {
            Debug.Log("Not enough insight to build.");
            return;
        }

        CommuneManager.s_instance.insightResource -= currentBuilding.cost;

        GameObject newBuildingObj = Instantiate(buildingPrefab, targetTile.transform.position, Quaternion.identity);
        Building newBuilding = newBuildingObj.GetComponent<Building>();
        newBuilding.Place(gridPos);

        targetTile.isOccupied = true;
        CommuneManager.s_instance.AddBuilding(newBuilding);
        // And handle multi-tile buildings in the future

        Destroy(buildingPreview);
        currentBuilding = null;
    }
}
