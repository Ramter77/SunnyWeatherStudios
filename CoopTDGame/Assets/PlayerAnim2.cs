using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim2 : MonoBehaviour
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
            Debug.Log(InputManager.Instance.Vertical2  + " " + InputManager.Instance.Horizontal2);
            Debug.Log(InputManager.Instance.MouseInput2.y  + " " + InputManager.Instance.MouseInput2.x);


        #region Axis based animation
        animator.SetFloat("Vertical", InputManager.Instance.Vertical2, axisDamping, Time.deltaTime);
        animator.SetFloat("Horizontal", InputManager.Instance.Horizontal2, axisDamping, Time.deltaTime);
        #endregion

        #region Button based animation
        if (InputManager.Instance.isRunning2) {
            animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
        }
        else {
            animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
        }
        #endregion
    }
}
