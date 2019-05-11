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
    /* [SerializeField]
    private float jumpDamping = 0; */


    #region INPUT
    private float _verticalInput;
    private float _horizontalInput;
    private bool _runInput;
    private bool _jumpInput;
    #endregion

    /* [SerializeField] float m_JumpPower = 12f; */



    private PlayerController playC;
    private Animator animator;
    private CharacterController charController;
    private Rigidbody playerRB;
    #endregion

    void Awake()
    {
        playC = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        #region Get Input
        if (playC.Player_ == 0) {
            _verticalInput = InputManager.Instance.Vertical0;
            _horizontalInput = InputManager.Instance.Horizontal0;
            _runInput = InputManager.Instance.Run0;
            _jumpInput = InputManager.Instance.Jump0;
        }

        else if (playC.Player_ == 1) {
            _verticalInput = InputManager.Instance.Vertical1;
            _horizontalInput = InputManager.Instance.Horizontal1;
            _runInput = InputManager.Instance.Run1;
            _jumpInput = InputManager.Instance.Jump1;
        }

        else if (playC.Player_ == 2) {
            _verticalInput = InputManager.Instance.Vertical2;
            _horizontalInput = InputManager.Instance.Horizontal2;
            _runInput = InputManager.Instance.Run2;
            _jumpInput = InputManager.Instance.Jump2;
        }
        #endregion


        #region Set Input
        #region Axis based animation
        animator.SetFloat("Vertical", _verticalInput, axisDamping, Time.deltaTime);
        animator.SetFloat("Horizontal", _horizontalInput, axisDamping, Time.deltaTime);
        #endregion

        #region Run
        if (_runInput) {
            animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
        }
        else {
            animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
        }
        #endregion

        #region Grounded Check & Jumping
        if (charController.isGrounded) {
            animator.SetBool("isGrounded", true);
            //playC.isJumping = false;
            //animator.applyRootMotion = true;
            //animator.SetFloat("isJumping", 1);

            if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && !playC.isJumping && !playC.isDead) {
                _Jump(_jumpInput);
            }

            //Jump(_jumpInput);
            /* if (_jumpInput) {
                animator.SetTrigger("Jump");
                playC.isJumping = true;
            } */
            //animator.SetFloat("isJumping", 0);
        }
        else {
            animator.SetBool("isGrounded", false);
            //animator.SetFloat("isJumping", playerRB.velocity.y);
        }
        
        #endregion
        #endregion
    }

    public void resetJumpingCD() {
        playC.isJumping = false;
    }

    void _Jump(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump)// && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            //Debug.Log("JUMP");
            /* playerRB.velocity = new Vector3(playerRB.velocity.x, m_JumpPower, playerRB.velocity.z);
            animator.SetBool("isGrounded", false); */
            //animator.applyRootMotion = false;
            //m_GroundCheckDistance = 0.1f;


            animator.GetComponent<MeleeAttack>().ActivateWeaponCollider();
            animator.SetTrigger("Jump");
            playC.isJumping = true;
        }
    }

    /* void Jump()
    {
        if (InputManager.Instance.Jump) {
            //playerAnim.animator.SetTrigger("Jump");

            
            if (!isJumping) {
                isJumping = true;

                if (playerAnim.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
                    isJumping = false;
                }
            }
           
        }
    }
    */
}