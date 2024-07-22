using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [BoxGroup("Base informations")]
    [Label("Name of building")]
    public string Name;

    [BoxGroup("Base informations")]
    [Label("Description")]
    [ResizableTextArea]
    public string Description;

    [BoxGroup("Base informations")]
    [Label("Item Cost Of Building")]
    public List<Cost> Cost;

    [BoxGroup("Base informations")]
    [Label("Sprite of building")]
    public Sprite Sprite;

    [BoxGroup("Building Size")]
    [Label("Tile Occupation Size")]
    [InfoBox("Grid size of building by x and y")]
    public Vector2 TileOccupationSize;

    [BoxGroup("Building Size")]
    [Label("Tile Occupation Position")]
    [InfoBox("Start Position Offset Of Building (bottom left corner)")]
    public Vector2 TileOccupationPosition;
}