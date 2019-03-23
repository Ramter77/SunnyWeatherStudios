using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Vertical", GameManagers.Instance.InputManager.Vertical);
        animator.SetFloat("Horizontal", GameManagers.Instance.InputManager.Horizontal);
    
        animator.SetBool("isRunning", GameManagers.Instance.InputManager.isRunning);
    }

    public void Jump() {
        animator.SetTrigger("Jump");
    }
}
