using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header ("Player")]
    [Tooltip ("Player_0 = Mouse, Player_1 = Controller 1, Player_2 = Controller 2")]
    public int Player_ = 1;
    [SerializeField] bool debugMode;
    [SerializeField] bool TurnPlayerForward;
    [SerializeField] float turn_Speed;
    [SerializeField] float groundCheckRadius = 1;
    [SerializeField] float groundCheckHeight = 1.5f;
    [SerializeField] int groundCheckDistance = 2;
    [SerializeField] bool movePlayerTowardSlope;
    [SerializeField] float slopeForce;
    [SerializeField] float slopeForceRayLength;

    #region STATES
    [Header ("Player STATES")]
    public bool isInBuildMode;
    public bool isMeleeAttacking;
    public bool isRangedAttacking;
    public bool isGrounded;
    public bool isJumping;
    public bool isDead;
    #endregion 

    private Transform MainCameraTransform;
    private PlayerAnim playerAnim;
    [HideInInspector]
    public AudioSource audioSource;
    CharacterController charController;
    RaycastHit groundHit;
    #endregion

    void Awake()
    {
        #region Initalize
        //GameManager.Instance.LocalPlayer = this;
        #endregion

        //If player 1 get MainCamera, else get MainCamera2
        string tag = "MainCamera";
        if (Player_ == 2)
        {
            tag = "MainCamera2";
        }
        if (debugMode) { Debug.Log("Finding " + tag + " tag"); }
        MainCameraTransform = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>().transform;
        
        playerAnim = GetComponent<PlayerAnim>();
        audioSource = GetComponent<AudioSource>();
        charController = GetComponent<CharacterController>();
    }

    private void Start() {
        isInBuildMode = false;
        isMeleeAttacking = false;
        isRangedAttacking = false;
        isJumping = false;
    }

    private void FixedUpdate() {
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

    private void SmoothLookForward(){
        //Turn player to cameras look rotation
        Vector3 forward = MainCameraTransform.forward;
        forward.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), turn_Speed * Time.deltaTime);
    }

    void CheckGround() {
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckHeight, Vector3.down);

        if (Physics.SphereCast(ray, groundCheckRadius, out groundHit, groundCheckDistance)) {
            isGrounded = true;
            isJumping = false;
        }
        else {
            isGrounded = false;
        }
	}

    private bool OnSlope()
    {
        if (isJumping) {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }


    void OnDrawGizmos() {
        if(debugMode) {
            #region CheckGround
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
            Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
            #endregion

            #region OnSlope
            Gizmos.DrawRay(transform.position, Vector3.down * charController.height / 2 * slopeForceRayLength);
            #endregion
        }
    }
}