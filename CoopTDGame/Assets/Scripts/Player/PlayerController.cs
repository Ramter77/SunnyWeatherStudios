using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player")]
    //! Handle players states here? attacking, jumping n all that
    public int Player_ = 1;
    public bool TurnPlayerForward;
    public float turn_Speed;

    #region Variables
    private Transform MainCameraTransform;
    private PlayerAnim playerAnim;

    #region STATES
    public bool isInBuildMode;
    public bool isMeleeAttacking;
    public bool isRangedAttacking;
    public bool isJumping;
    #endregion 
    #endregion

    void Awake()
    {
        #region Initalize
        GameManager.Instance.LocalPlayer = this;
        #endregion

        string tag = "MainCamera";
        if (Player_ == 2)
        {
            tag = "MainCamera2";
        }
        Debug.Log("Finding " + tag + " tag");
        MainCameraTransform = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>().transform;
        
        playerAnim = GetComponent<PlayerAnim>(); 
    }

    private void Start() {
        isMeleeAttacking = false;
        isRangedAttacking = false;
    }

    private void SmoothLookForward(){
        Vector3 forward = MainCameraTransform.forward;
        forward.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), turn_Speed * Time.deltaTime);
    }

    private void FixedUpdate() {
        //Turn Player1 forward (for Controllers its still on FreeCameraLook2 script)
        if (Player_ == 1) {
            if (TurnPlayerForward) {
                SmoothLookForward();
            }
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
}