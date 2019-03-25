using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddScriptToAllChildrenScript))]
[CanEditMultipleObjects]
public class AddScriptToAllChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AddScriptToAllChildrenScript script = (AddScriptToAllChildrenScript)target;

        if(GUILayout.Button("Add material editor script to all children"))
        {
            script.AddMaterialToSelectedGameObjects();
        }

        if(GUILayout.Button("Add collider editor script to all children"))
        {
            script.AddColliderToSelectedGameObjects();
        }

        if(GUILayout.Button("Add rigidbody editor script to all children"))
        {
            script.AddRigidbodyToSelectedGameObjects();
        }
    }
}
