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
    [Tooltip ("Damping for axis based animation for controllers")]
    /* [SerializeField]
    private float controllerDamping = 0;
    [Tooltip ("0 damping for jumping")] */
    [SerializeField]
    private float jumpDamping = 0;



    private PlayerController playC;
    private Animator animator;
    private CharacterController charController;
    private Rigidbody rb;
    #endregion

    void Awake()
    {
        playC = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        #region Grounded Check
        if (charController.isGrounded) {
            animator.SetBool("isGrounded", true);
        }
        else {
            animator.SetBool("isGrounded", false);
        }
        #endregion

        if (playC.Player_ == 0) {
            #region Axis based animation
            animator.SetFloat("Vertical", InputManager.Instance.Vertical, axisDamping, Time.deltaTime);
            animator.SetFloat("Horizontal", InputManager.Instance.Horizontal, axisDamping, Time.deltaTime);
            #endregion

            #region Button based animation
            //JUMP
            if (InputManager.Instance.Jump) {
                animator.SetFloat("isJumping", 1, jumpDamping, Time.deltaTime);
            }
            else {
                animator.SetFloat("isJumping", 0, jumpDamping, Time.deltaTime);
            }

            //RUN
            if (InputManager.Instance.isRunning) {
                animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
            }
            else {
                animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
            }
            #endregion
        }

        if (playC.Player_ == 1) {
            #region Axis based animation
            animator.SetFloat("Vertical", InputManager.Instance.Vertical1, axisDamping, Time.deltaTime);
            animator.SetFloat("Horizontal", InputManager.Instance.Horizontal1, axisDamping, Time.deltaTime);
            #endregion

            #region Button based animation
            //JUMP
            if (InputManager.Instance.Jump1) {
                animator.SetFloat("isJumping", 1, jumpDamping, Time.deltaTime);
            }
            else {
                animator.SetFloat("isJumping", 0, jumpDamping, Time.deltaTime);
            }

            //RUN
            if (InputManager.Instance.isRunning1) {
                animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
            }
            else {
                animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
            }
            #endregion
        }

        else if (playC.Player_ == 2) {
            #region Axis based animation
            animator.SetFloat("Vertical", InputManager.Instance.Vertical2, axisDamping, Time.deltaTime);
            animator.SetFloat("Horizontal", InputManager.Instance.Horizontal2, axisDamping, Time.deltaTime);
            #endregion

            //RUN
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

    void Jump()
    {
        if (InputManager.Instance.Jump) {
            //playerAnim.animator.SetTrigger("Jump");

            /*
            if (!isJumping) {
                isJumping = true;

                if (playerAnim.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
                    isJumping = false;
                }
            }
            */
        }
    }

    public void Land() {

    }
}