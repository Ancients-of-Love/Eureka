#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridManager grid = (GridManager)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            grid.GenerateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            grid.DestroyGrid();
        }
    }
}

#endif