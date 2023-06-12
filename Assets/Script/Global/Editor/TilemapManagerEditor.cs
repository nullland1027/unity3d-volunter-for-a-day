using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapManager))]
public class TilemapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (TilemapManager) target;

        if (GUILayout.Button("Save Map"))
        {
            script.SaveMap();
        }
        if (GUILayout.Button("Clear Map"))
        {
            script.ClearMap();
        }
        if (GUILayout.Button("Load Map"))
        {
            script.LoadMap();
        }
    }
    
}
