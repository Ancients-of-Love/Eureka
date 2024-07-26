using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [BoxGroup("Layout")]
    [SerializeField]
    public int Height;

    [BoxGroup("Layout")]
    [SerializeField]
    public int Width;

    [BoxGroup("Layout")]
    [SerializeField]
    public Tile Tile;

    public Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
    private List<Tile> lastHoveredTiles = null;
    private List<Tile> currentHoveredTiles = null;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Tile tile = transform.GetChild(i).GetComponent<Tile>();
            Tiles[new(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y))] = tile;
        }
    }

    private void Update()
    {
#if DEBUG
        GetHovering();
#endif
    }

    /// <summary>
    /// Generates grid by size in settings
    /// </summary>
    public void GenerateGrid()
    {
        Vector2 startPosition = new(0 - Width / 2, 0 - Height / 2);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tiles[new Vector2(x, y)] = Instantiate(Tile, new Vector3(startPosition.x + x, startPosition.y + y, 0), Quaternion.identity, transform);
            }
        }

        GenerateBiomes();
        GenerateResources();
    }

    private void GenerateBiomes()
    {
        //TODO: IMPLEMENT
    }

    private void GenerateResources()
    {
        //TODO: IMPLEMENT
    }

    /// <summary>
    /// Destroys current grid by removing all child Tile components
    /// </summary>
    public void DestroyGrid()
    {
        if (Tiles.Count <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Tile tile = transform.GetChild(i).GetComponent<Tile>();
                Tiles[new(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y))] = tile;
            }
        }

        foreach (var tile in Tiles)
        {
            tile.Value.DestoryTile();
        }
        Tiles.Clear();
    }

    /// <summary>
    /// Returns one hovered tile on specific position.
    /// </summary>
    public Tile GetTileAtPosition(Vector2 position)
    {
        Vector2 tempPosition = new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        if (Tiles.TryGetValue(tempPosition, out Tile tile))
        {
            return tile;
        }
        return null;
    }

    /// <summary>
    /// Returns all hovered tiles by size at cursor current position.
    /// </summary>
    public List<Tile> GetHoveringTiles(Vector2 SelectSize)
    {
        Vector3 mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 position = new(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
        List<Tile> tiles = GetTileOrNeighboursBySize(position, new Vector2(2, 2));
        return tiles;
    }

    /// <summary>
    /// Returns tile and neighbours by size at position.
    /// </summary>
    public List<Tile> GetTileOrNeighboursBySize(Vector2 position, Vector2 size)
    {
        List<Tile> neighbours = new();
        Tile centerTile = GetTileAtPosition(position);

        if (size.x == 1 && size.y == 1)
        {
            return new() { centerTile };
        }

        Vector2 startPosition = new(centerTile.transform.localPosition.x - Mathf.RoundToInt(size.x / 2), centerTile.transform.localPosition.y - Mathf.RoundToInt(size.y / 2));
        if (centerTile == null)
        {
            return null;
        }

        neighbours.Add(centerTile);
        neighbours.Add(GetTileAtPosition(startPosition));

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                Tile temp = GetTileAtPosition(startPosition + new Vector2(x, y));
                if (temp != null)
                {
                    neighbours.Add(temp);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Sets current hovering tiles to occupied
    /// </summary>
    public List<Tile> BuildOnTiles()
    {
        if (currentHoveredTiles == null)
        {
            return null;
        }
        foreach (Tile tile in currentHoveredTiles)
        {
            tile.IsOccupied = true;
        }
        return currentHoveredTiles;
    }

    /// <summary>
    /// Sets building's tiles to non-occupied
    /// </summary>
    public void DestroyBuildingOnTiles(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            tile.IsOccupied = false;
        }
    }

    /// <summary>
    /// TEST CODE FOR DEBUG
    /// </summary>
    private void GetHovering()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentHoveredTiles = GetHoveringTiles(SelectSize: new(1, 1));
        }

        if (currentHoveredTiles == null)
        {
            return;
        }

        if (lastHoveredTiles == null || lastHoveredTiles.Count == 0)
        {
            lastHoveredTiles = currentHoveredTiles;
            foreach (Tile tile in lastHoveredTiles)
            {
                tile.IsHovering = true;
            }
        }
        if (currentHoveredTiles != lastHoveredTiles)
        {
            foreach (Tile tile in lastHoveredTiles)
            {
                tile.IsHovering = false;
            }
            foreach (Tile tile in currentHoveredTiles)
            {
                tile.IsHovering = true;
            }
            lastHoveredTiles = currentHoveredTiles;
        }
    }
}