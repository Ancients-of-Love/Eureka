using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    [SerializeField]
    private List<GameObject> Buildables = new();

    [SerializeField]
    private GameObject Panel;

    [SerializeField]
    private GameObject BuildPanelButtonPrefab;
    private List<BuildSelectButton> buildSelectButtons = new();

    private int SelectedBuilding = 0;
    private GameObject CurrentSelectedBuildingPrefab = null;
    private bool IsBuildMode = false;

    private Vector3 BuildPosition = Vector3.zero;

    private void Start()
    {
        for (int i = 0; i < Buildables.Count; i++)
        {
            Debug.Log($"Added button to list! {i}");
            var buildingPanelButton = Instantiate(BuildPanelButtonPrefab, Panel.transform).GetComponent<BuildSelectButton>();
            buildingPanelButton.AssignedBuildingNumber = i;
            buildingPanelButton.BuildingSO = Buildables[i].GetComponent<Building>().BuildingSO;
            buildSelectButtons.Add(buildingPanelButton);
        }
        Panel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsBuildMode = !IsBuildMode;
            if (IsBuildMode)
            {
                Camera.main.orthographicSize = 7;
                Panel.SetActive(true);
            }
        }

        Vector3 mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, 0);
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        BuildPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        if (Input.GetMouseButtonDown(1) && IsBuildMode && CurrentSelectedBuildingPrefab == null && Panel.activeInHierarchy)
        {
            IsBuildMode = false;
        }

        if (!IsBuildMode)
        {
            if (CurrentSelectedBuildingPrefab != null)
            {
                Destroy(CurrentSelectedBuildingPrefab);
                CurrentSelectedBuildingPrefab = null;
            }
            Panel.SetActive(false);

            Camera.main.orthographicSize = 5;
            return;
        }

        if (Input.GetMouseButtonDown(1) && CurrentSelectedBuildingPrefab != null)
        {
            Destroy(CurrentSelectedBuildingPrefab);
            CurrentSelectedBuildingPrefab = null;
            Panel.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) && CurrentSelectedBuildingPrefab != null)
        {
            Debug.Log("BUILD!");
            BuildBuilding();
        }

        ManageSelectedBuilding();
    }

    private void ManageSelectedBuilding()
    {
        if (CurrentSelectedBuildingPrefab != null)
        {
            var building = CurrentSelectedBuildingPrefab.GetComponent<Building>();

            var color = CurrentSelectedBuildingPrefab.GetComponentInChildren<SpriteRenderer>().color;
            CurrentSelectedBuildingPrefab.GetComponentInChildren<SpriteRenderer>().color = new Vector4(255, 255, 255, 40f);
            List<Tile> tiles = GridManager.Instance.GetTileOrNeighboursBySize(BuildPosition, building.Size);
            Vector3 position = new(tiles[0].transform.position.x, tiles[0].transform.position.y, 0);
            CurrentSelectedBuildingPrefab.transform.position = position - (Vector3)building.Offset;
        }
    }

    private void BuildBuilding()
    {
        if (!CurrentSelectedBuildingPrefab.TryGetComponent<IBuilding>(out var building))
        {
            return;
        }

        List<Tile> tiles = GridManager.Instance.GetTileOrNeighboursBySize(BuildPosition, building.Size);
        foreach (var tile in tiles)
        {
            if (tile.IsOccupied)
            {
                return;
            }
        }
        building.BuildBuildingAt(tiles);

        CurrentSelectedBuildingPrefab = null;
        Panel.SetActive(true);
    }

    public void SetSelectedBuilding(int number)
    {
        if (number > Buildables.Count)
        {
            return;
        }
        SelectedBuilding = number;
        CurrentSelectedBuildingPrefab = Instantiate(Buildables[number]);
        Panel.SetActive(false);
    }

    //, Camera.main.ScreenToWorldPoint(mousePosition) + new Vector3(building.Offset.x, building.Offset.y, 0), Quaternion.identity, null
}