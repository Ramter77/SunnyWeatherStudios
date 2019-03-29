using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddColliderToSelectedGameObjectsScript))]
[CanEditMultipleObjects]
public class AddColliderToSelectedGameObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AddColliderToSelectedGameObjectsScript script = (AddColliderToSelectedGameObjectsScript)target;

        if(GUILayout.Button("Add Collider to all selected GameObjects"))
        {
            script.AddColliderToSelectedGameObjects();
        }
    }
}