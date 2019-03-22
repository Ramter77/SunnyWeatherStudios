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
    }

    [SerializeField] float speed;
    [SerializeField] MouseInput MouseControl;

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

    void Awake()
    {
        playerInput = GameManagers.Instance.InputManager;
        GameManagers.Instance.LocalPlayer = this;
    }

    void Update() {
        Vector2 direction = new Vector2(playerInput.Vertical * speed, playerInput.Horizontal * speed);
        MovementCont.Move(direction);

        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
    }
}