using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    public float axisDamping = 0.1f;
    public float buttonDamping = 0.5f;

    private Rigidbody rb;
    private float playerSpeed;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        animator.SetFloat("Vertical", GameManagers.Instance.InputManager.Vertical, axisDamping, Time.deltaTime);
        animator.SetFloat("Horizontal", GameManagers.Instance.InputManager.Horizontal, axisDamping, Time.deltaTime);
    
        //animator.SetBool("isRunning", GameManagers.Instance.InputManager.isRunning);


        if (GameManagers.Instance.InputManager.isRunning) {
            animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
        }
        else {
            animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
        }
    }
}
