using System;
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

        //only invisible for now
        public bool LockMouse;
    }

    #region Public Variables
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] MouseInput MouseControl;
    #endregion

    private MovementCont m_MovementCont;
    public MovementCont MovementCont {
        get {
            if (m_MovementCont == null) {
                m_MovementCont = GetComponent<MovementCont>();
            }
            return m_MovementCont;
        }
    }

    InputManager playerInput;
    Vector2 mouseInput;


    PlayerAnim playerAnim;
    private bool isJumping;

    void Awake()
    {
        playerInput = GameManagers.Instance.InputManager;
        GameManagers.Instance.LocalPlayer = this;

        playerAnim = GetComponent<PlayerAnim>();

        if (MouseControl.LockMouse) {
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
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
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
    }

    void Jump()
    {
        if (playerInput.Jump) {
            if (!isJumping) {
                isJumping = true;

                //TODO: make player change Y
                playerAnim.Jump();
            }
        }
    }
}