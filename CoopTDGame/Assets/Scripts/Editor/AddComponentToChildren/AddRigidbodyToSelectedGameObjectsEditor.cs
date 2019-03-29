using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddRigidbodyToSelectedGameObjectsScript))]
[CanEditMultipleObjects]
public class AddRigidbodyToSelectedGameObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AddRigidbodyToSelectedGameObjectsScript script = (AddRigidbodyToSelectedGameObjectsScript)target;

        if(GUILayout.Button("Add Rigidbody to all selected GameObjects"))
        {
            script.AddRigidbodyToSelectedGameObjects();
        }
    }
}