using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    #region Variables
    public Animator animator;

    [Tooltip ("Damping for axis based animation")]
    public float axisDamping = 0.1f;
    [Tooltip ("Damping for button based animation")]
    public float buttonDamping = 0.5f;

    private Rigidbody rb;
    private float playerSpeed;
    #endregion

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        #region Axis based animation
        animator.SetFloat("Vertical", InputManager.Instance.Vertical, axisDamping, Time.deltaTime);
        animator.SetFloat("Horizontal", InputManager.Instance.Horizontal, axisDamping, Time.deltaTime);
        #endregion

        #region Button based animation
        if (InputManager.Instance.isRunning) {
            animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
        }
        else {
            animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
        }
        #endregion
    }
}
