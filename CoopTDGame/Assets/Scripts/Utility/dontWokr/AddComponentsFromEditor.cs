using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddComponentsFromScript))]
[CanEditMultipleObjects]
public class AddComponentsFromEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AddComponentsFromScript script = (AddComponentsFromScript)target;

        if(GUILayout.Button("Add Material to all selected GameObjects"))
        {
            script.AddMaterialToSelectedGameObjects();
        }

        if(GUILayout.Button("Add Colliders to all selected GameObjects"))
        {
            script.AddCollidersToSelectedGameObjects();
        }

        if(GUILayout.Button("Add RigidBodies to all selected GameObjects"))
        {
            script.AddRigidBodiesToSelectedGameObjects();
        }
    }
}
