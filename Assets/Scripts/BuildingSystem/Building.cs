using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public abstract class Building : MonoBehaviour, IBuilding
{
    [SerializeField]
    private BuildingSO BuildingSO;

    [NonSerialized]
    public string Name;

    [NonSerialized]
    public string Desc;

    [NonSerialized]
    public List<Cost> Cost;

    [NonSerialized]
    public Sprite Sprite;

    [NonSerialized]
    public Vector2 Size;

    [NonSerialized]
    public Vector2 Offset;

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

    //PILL
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

    public abstract Vector3 GetBuildingPosition();

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
}