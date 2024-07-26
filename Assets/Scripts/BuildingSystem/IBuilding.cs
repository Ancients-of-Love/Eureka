using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public Vector2 Size { get; }
    public Vector2 Offset { get; }

    public Vector3 GetBuildingPosition();

    public void InteractWithBuilding();

    public void BuildBuildingAt(List<Tile> tiles);

    public void DestroyBuilding();
}