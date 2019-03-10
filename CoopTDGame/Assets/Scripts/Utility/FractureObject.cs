using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject FracturedObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Instantiate(FracturedObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void Fracture() {
        Instantiate(FracturedObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
