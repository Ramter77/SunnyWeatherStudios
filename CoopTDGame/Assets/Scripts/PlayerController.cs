using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player")]
    //! Handle players states here? attacking, jumping n all that
    public int Player_ = 1;
    public bool TurnPlayerForward;

    #region Variables
    private Transform MainCameraTransform;
    Vector3 CameraMForward;
    Vector3 moveInput;
    float rotationAmount;

    private PlayerAnim playerAnim;
    private bool isJumping;

    public float turn_Speed;
    #endregion

    void Awake()
    {
        #region Initalize
        GameManager.Instance.LocalPlayer = this;
        #endregion

        if (Player_ == 1) {
            Debug.Log("Finding MainCamera tag");
            MainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform;
        }
        else
        {
            Debug.Log("Finding MainCamera2 tag");
            MainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera2").GetComponent<Camera>().transform;
        }
        
        playerAnim = GetComponent<PlayerAnim>(); 
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