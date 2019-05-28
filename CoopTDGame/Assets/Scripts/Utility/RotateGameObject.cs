using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    [SerializeField]
    private float RotationSpeed = 1;
    [SerializeField]
    public bool rotateSidewards = false;

    void Update()
    {
        if(rotateSidewards)
            transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
        else 
        transform.Rotate(Vector3.up * ( RotationSpeed * Time.deltaTime));
    }
}
