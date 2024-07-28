using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Building : MonoBehaviour, IBuilding
{
    [SerializeField]
    public BuildingSO BuildingSO;

    [NonSerialized]
    public string Name;

    [NonSerialized]
    public string Desc;

    [NonSerialized]
    public List<ItemAmount> Cost;

    public Sprite Sprite { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 Offset { get; private set; }

    public bool IsActive { get; private set; }

    private SpriteRenderer SpriteRenderer;
    private List<Tile> OccupiedTiles;
    private BoxCollider PhysicsBox;
    private SphereCollider InteractCollider;

    protected void Awake()
    {
        InitializeBuilding();
    }

    protected void Start()
    {
        RenderBuilding();
    }

    private void InitializeBuilding()
    {
        if (BuildingSO == null)
        {
            return;
        }
        Name = BuildingSO.Name;
        Desc = BuildingSO.Name;
        Cost = BuildingSO.Cost;
        Sprite = BuildingSO.Sprite;
        Size = BuildingSO.TileOccupationSize;
        Offset = BuildingSO.TileOccupationPosition;

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PhysicsBox = GetComponent<BoxCollider>();
        InteractCollider = GetComponent<SphereCollider>();
        OccupiedTiles = new List<Tile>();
        PhysicsBox.size = Size;
        InteractCollider.radius = Size.x;
        IsActive = false;
    }

    private void RenderBuilding()
    {
        if (Sprite == null || SpriteRenderer == null)
        {
            return;
        }

        SpriteRenderer.sprite = Sprite;
    }

    public abstract void InteractWithBuilding();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 7)
       {
            PlayerInteractManager.Instance.AddNearBuilding(this);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 7)
        {
            PlayerInteractManager.Instance.RemoveNearBuilding(this);
        }
    }

    public void BuildBuildingAt(List<Tile> tiles)
    {
        if (!InventoryManager.Instance.HasEnoughItems(Cost))
        {
            return;
        }
        OccupiedTiles = tiles;
        foreach (Tile tile in OccupiedTiles)
        {
            tile.IsOccupied = true;
        }
        Vector3 position = new(tiles[0].transform.position.x, tiles[0].transform.position.y, 0);
        transform.position = position - (Vector3)Offset;
        var color = SpriteRenderer.color;
        SpriteRenderer.color = new Vector4(255, 255, 255, 255f);
        InventoryManager.Instance.RemoveItem(Cost[0].Item, Cost[0].Amount);
        IsActive = true;
    }

    public void DestroyBuilding()
    {
        foreach (Tile tile in OccupiedTiles)
        {
            tile.IsOccupied = false;
        }
        OccupiedTiles.Clear();

        Destroy(gameObject);
    }

    public virtual Vector3 GetBuildingPosition()
    {
        return transform.position;
    }
}