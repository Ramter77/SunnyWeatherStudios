using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToGameManager : MonoBehaviour
{
    public GameObject gm;

    void Start()
    {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GameManager");
        }
        transform.parent = gm.transform;
    }
}
