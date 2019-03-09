using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddComponentsFromScript : MonoBehaviour
{
    //Script gets executed on button press of the AddCollidersnRBsToSelectedGameObjectsEditor script
    [Header ("Material")]
    public Material material;

    [Header ("Mesh Collider")]
    public bool convex = true;

    [Header ("Rigidbody")]
    public int mass = 1;
    public int drag = 0, angularDrag = 0;

    public void AddMaterialToSelectedGameObjects() {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region Materials
            MeshRenderer MeshRenderer = go.GetComponent<MeshRenderer>();
            if (material) {
                MeshRenderer.material = material;
            }
            #endregion
        }
    }

    public void AddCollidersToSelectedGameObjects() {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region MeshCollider
            MeshCollider meshCollider = go.AddComponent(typeof(MeshCollider)) as MeshCollider;
            if (convex) {
                meshCollider.convex = true;
            }
            #endregion
        }
    }

    public void AddRigidBodiesToSelectedGameObjects() {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region Rigidbody
            Rigidbody rb = go.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.mass = mass;
            rb.drag = drag;
            rb.angularDrag = angularDrag;
            #endregion
        }
    }
}
