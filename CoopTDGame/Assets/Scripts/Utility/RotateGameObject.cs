using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    public float RotationSpeed = 1;

    public bool playAnimationToo;
    private Animation anim;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animation>();

        if (playAnimationToo) {
            anim.Play();
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * ( RotationSpeed * Time.deltaTime));
    }
}
