using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScriptToAllChildrenScript : MonoBehaviour
{
    public void AddMaterialToSelectedGameObjects() {
        //Add material script to all children
        foreach (Transform child in transform)
        {
            if (!child.gameObject.GetComponent<AddMaterialToSelectedGameObjectsScript>()) {
                AddMaterialToSelectedGameObjectsScript script = child.gameObject.AddComponent(typeof(AddMaterialToSelectedGameObjectsScript)) as AddMaterialToSelectedGameObjectsScript;
            }
        }
    }

    public void AddColliderToSelectedGameObjects() {
        //Add collider script to all children
        foreach (Transform child in transform)
        {
            if (!child.gameObject.GetComponent<AddColliderToSelectedGameObjectsScript>()) {
                if (!child.gameObject.GetComponent<MeshCollider>()) {
                    AddColliderToSelectedGameObjectsScript script = child.gameObject.AddComponent(typeof(AddColliderToSelectedGameObjectsScript)) as AddColliderToSelectedGameObjectsScript;
                }
            }
        }
    }

    public void AddRigidbodyToSelectedGameObjects() {
        //Add rigidbody script to all children
        foreach (Transform child in transform)
        {
            if (!child.gameObject.GetComponent<AddRigidbodyToSelectedGameObjectsScript>()) {
                if (!child.gameObject.GetComponent<Rigidbody>()) {
                    AddRigidbodyToSelectedGameObjectsScript script = child.gameObject.AddComponent(typeof(AddRigidbodyToSelectedGameObjectsScript)) as AddRigidbodyToSelectedGameObjectsScript;
                }
            }
        }
    }
}
