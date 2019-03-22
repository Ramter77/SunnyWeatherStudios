using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    public float RotationSpeed = 1;
    void Update()
    {
        transform.Rotate(Vector3.up * ( RotationSpeed * Time.deltaTime));
    }
}
