using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [BoxGroup("Enums")]
    [Label("Tile's biome")]
    [SerializeField]
    public BiomeEnum Biome;

    [BoxGroup("Enums")]
    [Label("Type of tile")]
    [SerializeField]
    public TileTypeEnum TileType;

    [Label("Is building allowed")]
    [SerializeField]
    public bool IsBuildAllowed;

    public bool IsOccupied;
    public bool IsHovering;
    public bool IsOnCamera;
    public int DarknessLevel;

    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private GameObject test;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        DarknessLevel = 0;

        IsOccupied = false;
        IsOccupied = false;
        IsOccupied = false;
        IsHovering = false;
        IsOnCamera = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsOnCamera)
        {
            return;
        }

        if (IsHovering)
        {
            SpriteRenderer.enabled = true;
        }
        else
        {
            SpriteRenderer.enabled = false;
        }
    }

    public void DestoryTile()
    {
        DestroyImmediate(gameObject);
    }
}