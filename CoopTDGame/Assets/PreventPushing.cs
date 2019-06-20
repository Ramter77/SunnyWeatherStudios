using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventPushing : MonoBehaviour
{
    private Rigidbody rb;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
            rb.isKinematic = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
            rb.isKinematic = false;
        }
    }
}
