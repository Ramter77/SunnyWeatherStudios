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
    [SerializeField]
    private bool enableWeaponColliderOnJump;
    [SerializeField]
    private bool toggleRunning;
    [SerializeField]
    private bool allowJumpWhileMelee;
    [SerializeField]
    private bool restrictForwardJump;
    [SerializeField]
    private bool allowJumpWhileRanged;
    private bool allowJump;


    #region INPUT
    private float _verticalInput;
    private float _horizontalInput;
    private bool _runInput;
    private bool _jumpInput;
    private bool toggleRun;
    #endregion

    private PlayerController playC;
    private Animator animator;
    private CharacterController charController;
    private Rigidbody playerRB;
    private CleanIK cleanIKscript;
    #endregion

    void Awake()
    {
        playC = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();
        cleanIKscript = GetComponent<CleanIK>();

        toggleRun = InputManager.Instance.toggleRun;
    }

    void Update()
    {
        if (!playC.isDead) {
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
        }

        #region Set Input
        #region Axis based animation
        animator.SetFloat("Vertical", _verticalInput, axisDamping, Time.deltaTime);
        animator.SetFloat("Horizontal", _horizontalInput, axisDamping, Time.deltaTime);
        #endregion

        #region Run
        if (toggleRun) {
            if (_runInput) {
                if (toggleRunning) {
                    animator.SetFloat("isRunning", 0, buttonDamping, Time.time);
                }
                else
                {
                    animator.SetFloat("isRunning", 1, buttonDamping, Time.time);
                }
                toggleRunning = !toggleRunning;
            }
        }
        else {
            if (_runInput) {
                animator.SetFloat("isRunning", 0, buttonDamping, Time.deltaTime);
            }
            else {
                animator.SetFloat("isRunning", 1, buttonDamping, Time.deltaTime);
            }
        }
        #endregion

        #region Grounded Check
        if (playC.isGrounded) {
            animator.SetBool("isGrounded", true);

            //Enable CleanIK if input is 0
            if (_verticalInput == 0 && _horizontalInput == 0) {
                cleanIKscript.enabled = true;
            }
            else
            {
                cleanIKscript.enabled = false;
            }

            #region Jumping
            if (/* !playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && */ !playC.isJumping) {
                if (!allowJumpWhileMelee && !allowJumpWhileRanged) {
                    if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode) {
                        allowJump = true;
                    }
                }

                else if (allowJumpWhileMelee && allowJumpWhileRanged) {
                    allowJump = true;
                }

                else if (allowJumpWhileMelee) {
                    if (!playC.isRangedAttacking) {
                        allowJump = true;
                    }
                }

                else if (allowJumpWhileRanged) {
                    if (!playC.isMeleeAttacking) {
                        allowJump = true;
                    }
                }

                if (allowJump) {
                    if (restrictForwardJump) {
                        if (playC.isMeleeAttacking || playC.isRangedAttacking) {
                            if (_verticalInput <= 0)  {
                                _Jump(_jumpInput);
                            }
                        }
                        else
                        {
                            _Jump(_jumpInput);
                        }
                    }
                    else
                    {
                        _Jump(_jumpInput);
                    }
                }
                
            }
            #endregion
        }
        else {
            animator.SetBool("isGrounded", false);
        }
        #endregion
        #endregion
    }

    public void resetJumpingCD() {
        playC.isJumping = false;
        playC.isRangedAttacking = false;
        animator.SetBool("Jumping", false);
    }

    void _Jump(bool jump)
    {
        


        if (jump)// && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (!playC.isRangedAttacking) {
            playC.isJumping = true;


            //remember to disable weapon collider after jump
            if (enableWeaponColliderOnJump) {
                animator.GetComponent<MeleeAttack>().ActivateWeaponCollider();
            }
            //Debug.Log(_verticalInput + " and " + Mathf.RoundToInt(_verticalInput));
            //! rounding to int to prevent weird blends, especially with controllers
            //! keep an eye on this: if it leads to problems use direct animation transitions

            //use a seperate float for directional jump blend tree
            animator.SetFloat("jumpVertical", Mathf.RoundToInt(_verticalInput));
            animator.SetFloat("jumpHorizontal", Mathf.RoundToInt(_horizontalInput));
            //animator.SetTrigger("Jump");
            animator.SetBool("Jumping", true);
            }
        }
    }
}