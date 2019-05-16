using UnityEngine;
using System.Collections;
 
public class FlyCamera : MonoBehaviour {

    [Header ("Mouse/WASD to move, space to ascend, left shift to speed up")]

    private Vector3 _angles;
    public float speed = 1.0f;
    public float ySpeedMultiplier = 0.5f;
    public float fastSpeed = 2.0f;
    public float mouseSpeed = 4.0f;

    private void OnEnable() {
        _angles = transform.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable() { Cursor.lockState = CursorLockMode.None; }

    private void Update() {
        _angles.x -= Input.GetAxis("MouseY0") * mouseSpeed;
        _angles.y += Input.GetAxis("MouseX0") * mouseSpeed;
        transform.eulerAngles = _angles;
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : speed;
        transform.position +=
            Input.GetAxis("Horizontal0") * moveSpeed * transform.right +
            Input.GetAxis("Vertical0") * moveSpeed * transform.forward;

        if (Input.GetButton("Jump0")) {
            transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * ySpeedMultiplier, transform.position.z);
        }
    }
}