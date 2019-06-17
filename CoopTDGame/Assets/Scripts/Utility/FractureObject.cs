using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject FracturedObject;
    [Tooltip ("Destroy or disable gameObject")]
    [SerializeField]
    private bool destroyObject = true;
    [Tooltip ("Press U to activate")]
    [SerializeField]
    private bool debug;

    // Update is called once per frame
    void Update()
    {
        if (debug) {
            if (Input.GetKeyDown(KeyCode.L)) {
                Instantiate(FracturedObject, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    public void Fracture(GameObject DestroyedObject) {
        if (destroyObject) {
            Instantiate(FracturedObject, DestroyedObject.transform.position, DestroyedObject.transform.rotation);
            Destroy(DestroyedObject);
        }
        else
        {
            //DestroyedObject.SetActive(false);
            DestroyedObject.gameObject.GetComponent<ActivatePrefab>()._LowerTower();
        }
    }
}
