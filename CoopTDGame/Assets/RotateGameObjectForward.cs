using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObjectForward : MonoBehaviour
{
    public float RotationSpeed = 20;
    void Update()
    {
        transform.Rotate(Vector3.forward * ( RotationSpeed * Time.deltaTime));
    }
}
