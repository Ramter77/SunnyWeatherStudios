using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddMaterialToSelectedGameObjectsScript))]
[CanEditMultipleObjects]
public class AddMaterialToSelectedGameObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AddMaterialToSelectedGameObjectsScript script = (AddMaterialToSelectedGameObjectsScript)target;

        if(GUILayout.Button("Add Material to all selected GameObjects"))
        {
            script.AddMaterialToSelectedGameObjects();
        }
    }
}