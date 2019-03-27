﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementCont))]
public class PlayerCont : MonoBehaviour
{
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
    PlayerAnim playerAnim;
    private bool isJumping;
    #endregion

    void Awake()
    {
        #region Initalize
        playerInput = GameManagers.Instance.InputManager;
        sceneControl = GameManagers.Instance.SceneControl;
        slowMotionControl = GameManagers.Instance.SlowMotionControl;

        GameManagers.Instance.LocalPlayer = this;
        #endregion

        playerAnim = GetComponent<PlayerAnim>();

/* 
        if (MouseControl.LockMouse) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        */
    }

    void Update() {
        Move();
        Look();
        Jump();
    }

    void Move() {
        //only move when not jumping
        if (!isJumping) {
            float moveSpeed = walkSpeed;

            if (playerInput.isRunning) {
                moveSpeed = runSpeed;
            }

            Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
            MovementCont.Move(direction);
        }
    }

    void Look()
    {
        //mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        //mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        //transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        //transform.Rotate(Vector3.up * mouseInput.y * MouseControl.Sensitivity.y);


        //Camera.main.transform.Rotate(Vector3.left * mouseInput.y * MouseControl.Sensitivity.y);
        
        //Camera.main.GetComponent<ThirdPersonCam>().cameraOffset.y += mouseInput.y * MouseControl.Sensitivity.y;

    }

    void Jump()
    {
        if (playerInput.Jump) {
            playerAnim.animator.SetTrigger("Jump");

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