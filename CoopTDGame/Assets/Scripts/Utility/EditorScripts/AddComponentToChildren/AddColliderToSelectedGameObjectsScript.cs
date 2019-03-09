using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class AddColliderToSelectedGameObjectsScript : MonoBehaviour
{
    //Script gets executed on button press of the AddColliderToSelectedGameObjectsEditor script
    [Header ("Mesh Collider")]
    public bool convex = true;
    private MeshCollider meshCollider;

    public void AddColliderToSelectedGameObjects()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region MeshCollider
            meshCollider = go.GetComponent<MeshCollider>();
            //If no meshRenderer then add one if enabled
            if (!meshCollider) {
                meshCollider = go.AddComponent(typeof(MeshCollider)) as MeshCollider;
                if (convex) {
                    meshCollider.convex = true;
                }
            }
            #endregion

            //Remove this script
            DestroyImmediate(GetComponent<AddColliderToSelectedGameObjectsScript>());
        }
    }  
}
