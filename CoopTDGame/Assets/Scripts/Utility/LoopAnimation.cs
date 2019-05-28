using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimation : MonoBehaviour
{
    [SerializeField]
    private bool playAnimationOnStart;
    private Animation anim;
    
    void Start()
    {
        anim = GetComponent<Animation>();

        if (playAnimationOnStart) {
            anim.Play();
        }
    }
}
