using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.MultiAudioListener;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public int Player = 1;

    [Header ("Player")]
    [Tooltip ("Player_0 = Mouse, Player_1 = Controller 1, Player_2 = Controller 2")]
    public int Player_ = 1;
    public Element Element;
    [SerializeField] bool debugMode;
    public CharacterController charController;
    public Transform MainCameraTransform;
    public MultiAudioSource playerAudioSource;

    [Header ("Parameters")]
    [Tooltip ("Layer mask used for all rays")]
    [SerializeField] LayerMask mask;

    [Space (10)]
    [SerializeField] bool TurnPlayerForward;
    [SerializeField] float turn_Speed;

    [Space (10)]
    [SerializeField] float groundCheckRadius = 1;
    [SerializeField] float groundCheckHeight = 1.5f;
    [SerializeField] int groundCheckDistance = 2;

    [Space (10)]
    [SerializeField] bool movePlayerTowardSlope;
    [SerializeField] float slopeForce;
    [SerializeField] float slopeForceRayLength;

    [Space (10)]
    [SerializeField] bool allowAirMovement;
    [SerializeField] private float airMoveSpeed;
    [SerializeField] bool allowJumpMovement;
    [SerializeField] private float jumpMoveSpeedMultiplier = 0.5f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveVelocity;
    private float _verticalInput;
    private float _horizontalInput;
    

    #region STATES
    [Header ("Player STATES")]
    public bool isCasting;
    public bool isMeleeAttacking;
    public bool isRangedAttacking;
    public bool isGrounded;
    public bool isJumping;
    public bool isDead;
    public bool isPaused;
    #endregion

    
    private PlayerAnim playerAnim;
    //[HideInInspector]
    //public MultiAudioSource audioSource;
    private RaycastHit groundHit;
    private Rigidbody rb;
    #endregion

    void Awake()
    {
        #region Initalize
        //GameManager.Instance.LocalPlayer = this;
        #endregion

        //If player 1 get MainCamera, else get MainCamera2
        /* string tag = "MainCamera";
        if (Player_ == 2)
        {
            tag = "MainCamera2";
        }
        if (debugMode) { Debug.Log("Finding " + tag + " tag"); } */
        //MainCameraTransform = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>().transform;
        
        playerAnim = GetComponent<PlayerAnim>();
        playerAudioSource = GetComponent<MultiAudioSource>();
        if (charController == null) { charController = GetComponent<CharacterController>(); }
        rb = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(gameObject.layer, 15);
    }

    private void Start() {
        isCasting = false;
        isMeleeAttacking = false;
        isRangedAttacking = false;
        isJumping = false;
    }

    private void Update() {
        if (!isDead && !isPaused) {
            //Turn Player forward
            if (TurnPlayerForward) {
                SmoothLookForward();
            }

            //Check if isGrounded
            CheckGround();

            //Move player towards slope
            if (movePlayerTowardSlope) {
                if (OnSlope())
                {
                    charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
                }
            }
        }
    }

    private void SmoothLookForward(){
        //Turn player to cameras look rotation
        Vector3 forward = MainCameraTransform.forward;
        forward.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), turn_Speed * Time.deltaTime);
    }

    void CheckGround() {
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckHeight, Vector3.down);

        if (Physics.SphereCast(ray, groundCheckRadius, out groundHit, groundCheckDistance, mask)) {
            isGrounded = true;
            //isJumping = false;

            if (allowJumpMovement) {
                //If playing Jump blend tree let player add movement
                if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpBlend")) {
                    ControlInAir(jumpMoveSpeedMultiplier);
                }
            }
        }
        else {
            isGrounded = false;

            if (allowAirMovement) {
                ControlInAir(1);
            }
        }
	}

    private void ControlInAir(float speedMultiplier)
    {
        #region Input
        //* Player 0 input */
        if (Player_ == 0)
        {
            _verticalInput = InputManager.Instance.Vertical0;
            _horizontalInput = InputManager.Instance.Horizontal0;
        }

        //* Player 1 input */
        else if (Player_ == 1)
        {
            _verticalInput = InputManager.Instance.Vertical1;
            _horizontalInput = InputManager.Instance.Horizontal1;
        }

        //*Player 2 input */
        else if (Player_ == 2) {
            _verticalInput = InputManager.Instance.Vertical2;
            _horizontalInput = InputManager.Instance.Horizontal2;
        }
        #endregion
        
        moveDirection = new Vector3(_horizontalInput, 0, _verticalInput);        
        moveDirection = transform.TransformDirection(moveDirection);
        moveVelocity = moveDirection.normalized * airMoveSpeed * speedMultiplier;

        charController.Move(moveVelocity * Time.deltaTime);

        //rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
    }

    private bool OnSlope()
    {
        if (isJumping) {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength, mask))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }


    void OnDrawGizmos() {
        if (debugMode) {
            #region CheckGround
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
            Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
            #endregion

            #region OnSlope
            if(charController != null) 
                Gizmos.DrawRay(transform.position, Vector3.down * charController.height / 2 * slopeForceRayLength);
            #endregion
        }
    }

    public void SetAlive() {
        isPaused = false;
        //isDead = false;
        MainCameraTransform.parent.transform.parent.transform.parent.gameObject.SetActive(true);
    }
}