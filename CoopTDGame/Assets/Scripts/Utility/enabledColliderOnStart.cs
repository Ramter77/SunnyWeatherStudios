using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enabledColliderOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<Collider>().enabled = true;
    }
}
