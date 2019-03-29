using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    #region Variables
    [Tooltip ("Damping for axis based animation")]
    [SerializeField]
    private float axisDamping = 0.1f;
    [Tooltip ("Damping for button based animation")]
    [SerializeField]
    private float buttonDamping = 0.5f;
    [Tooltip ("0 damping for controllers")]
    [SerializeField]
    private float controllerDamping = 0;

    private PlayerController playC;
    private Animator animator;
    private Rigidbody rb;
    #endregion

    void Awake()
    {
        playC = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playC.Player_ == 1) {
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

        else {
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
}