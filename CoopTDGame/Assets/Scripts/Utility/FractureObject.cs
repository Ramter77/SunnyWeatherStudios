using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject FracturedObject;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            Instantiate(FracturedObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void Fracture(GameObject DestroyedObject) {
        Instantiate(FracturedObject, DestroyedObject.transform.position, DestroyedObject.transform.rotation);
        Destroy(DestroyedObject);
    }
}
