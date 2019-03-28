using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{

    public int Player_ = 1;


    [System.Serializable]
    public class MouseInput {
        public Vector2 Damping;
        public Vector2 Sensitivity;

        public float minAngle = 1; 
        public float maxAngle = 15;

        //only invisible for now
        public bool LockMouse;
    }

    private MovementCont m_MovementCont;
    public MovementCont MovementCont {
        get {
            if (m_MovementCont == null) {
                m_MovementCont = GetComponent<MovementCont>();
            }
            return m_MovementCont;
        }
    }

    #region Variables
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] public MouseInput MouseControl;
    Vector2 mouseInput;


    InputManager playerInput;
    SceneControl sceneControl;
    SlowMotionControl slowMotionControl;

    private Camera CameraM;
    private Transform CameraMTransform;
    Vector3 CameraMForward;
    Vector3 moveInput;
    float rotationAmount;

    private PlayerAnim playerAnim;
    private bool isJumping;
    public float minRotation = 90;
    public float maxRotation = 180;
    public float m_Speed;
    #endregion

    void Awake()
    {
        #region Initalize


        GameManager.Instance.LocalPlayer = this;
        #endregion

        CameraM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CameraMTransform = CameraM.transform;


        playerAnim = GetComponent<PlayerAnim>();

/* 
        if (MouseControl.LockMouse) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            
        }
        */

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
    }

    

    void Update() {
        Move();
        Look();
        Jump();
    }

    void SmoothLook(Vector3 newDirection){
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), m_Speed * Time.deltaTime);
    }

    private void FixedUpdate() {


        /*  #region Camera Rotation
        CameraMForward = Vector3.Scale(CameraMTransform.forward, new Vector3(1, 0, 1)).normalized;
        moveInput = InputManager.Instance.MouseInput.x * CameraMForward + InputManager.Instance.MouseInput.x *CameraMTransform.right;
        //Debug.Log(InputManager.Instance.MouseInput.x + " " + InputManager.Instance.MouseInput.y + " >> " +moveInput);

        if (moveInput.magnitude > 1f) moveInput.Normalize();

        moveInput = transform.InverseTransformDirection(moveInput);
        rotationAmount = Mathf.Atan2(moveInput.x, moveInput.z);

        Debug.Log(InputManager.Instance.Vertical );

        float rotationSpeed = Mathf.Lerp(minRotation, maxRotation, InputManager.Instance.Vertical);
        transform.Rotate(0, rotationAmount * rotationSpeed * Time.deltaTime, 0);
        #endregion
 */

        //var rota = Camera.main.GetComponent<Transform>().forward;


        Vector3 ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(-1);

        //moveInput = transform.InverseTransformDirection(moveInput);
        Vector3 rayXZ = new Vector3(-ray.x, -transform.position.y, -ray.z);
        //transform.LookAt(rayXZ);

        /* Vector3 lTargetDir = rayXZ - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * m_Speed); */

        //SmoothLook(rayXZ - transform.position);



        Vector3 smoLok = Camera.main.transform.forward;
        smoLok.y = 0;

        if (Player_ == 1) {
        SmoothLook(smoLok);
        }

        //Vector3 fromRotation = transform.rotation.eulerAngles;
        //Vector3 newRotation = fromRotation + Vector3.up * ( rotationAmount * 360 * Time.deltaTime);

        //Debug.Log(newRotation);

        //transform.rotation.SetEulerAngles(newRotation);
        
        //Debug.Log("Vel: " + transform.GetComponent<Rigidbody>().velocity);
        //Debug.Log("Rot: " + rotationAmount * 360 * Time.deltaTime);
    }

    void Move() {

        


        //only move when not jumping
        if (!isJumping) {
            float moveSpeed = walkSpeed;

            if (InputManager.Instance.isRunning) {
                moveSpeed = runSpeed;
            }

            //Vector2 direction = new Vector2(InputManager.Instance.Vertical * moveSpeed, InputManager.Instance.Horizontal * moveSpeed);
            //MovementCont.Move(direction);
        }
    }

    void Look()
    {
        //mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        //mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        //transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        //transform.Rotate(Vector3.up * mouseInput.y * MouseControl.Sensitivity.y);


        //CameraM.transform.Rotate(Vector3.left * mouseInput.y * MouseControl.Sensitivity.y);
        
        //CameraM.GetComponent<ThirdPersonCam>().cameraOffset.y += mouseInput.y * MouseControl.Sensitivity.y;


        //Vector3 ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(100);


        //Vector3 ray = transform.
        //Vector3 rayXZ = new Vector3(ray.x, transform.position.y, ray.z);
        //transform.LookAt(InputManager.Instance.MouseInput);

        //transform.LookAt(InputManager.Instance.MouseInput.normalized);
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