using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header ("Player")]
    [Tooltip ("Player_0 = Mouse, Player_1 = Controller 1, Player_2 = Controller 2")]
    public int Player_ = 1;
    [SerializeField] bool TurnPlayerForward;
    [SerializeField] float turn_Speed;

    #region STATES
    [Header ("Player STATES")]
    public bool isInBuildMode;
    public bool isMeleeAttacking;
    public bool isRangedAttacking;
    public bool isJumping;
    public bool isDead;
    #endregion 

    private Transform MainCameraTransform;
    private PlayerAnim playerAnim;
    [HideInInspector]
    public AudioSource audioSource;
    CharacterController charController;
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;
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
        Debug.Log("Finding " + tag + " tag");
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
        if(OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }


    private void SmoothLookForward(){
        //Turn player to cameras look rotation
        Vector3 forward = MainCameraTransform.forward;
        forward.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), turn_Speed * Time.deltaTime);
    }
}