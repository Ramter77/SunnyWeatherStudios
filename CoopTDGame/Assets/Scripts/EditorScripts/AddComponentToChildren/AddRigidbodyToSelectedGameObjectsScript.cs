using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class AddRigidbodyToSelectedGameObjectsScript : MonoBehaviour
{
    //Script gets executed on button press of the AddRigidbodyToSelectedGameObjectsEditor script
    [Header ("Rigidbody")]
    public int mass = 1;
    public int drag = 0, angularDrag = 0;
    private Rigidbody rb;

    public void AddRigidbodyToSelectedGameObjects()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region Rigidbody
            rb = go.GetComponent<Rigidbody>();
            if (!rb) { 
                Rigidbody rb = go.AddComponent(typeof(Rigidbody)) as Rigidbody;
                rb.mass = mass;
                rb.drag = drag;
                rb.angularDrag = angularDrag;
            }
            #endregion

            //Remove this script
            DestroyImmediate(go.GetComponent<AddRigidbodyToSelectedGameObjectsScript>());
        }
    }
}
