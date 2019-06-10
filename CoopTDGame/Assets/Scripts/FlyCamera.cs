using UnityEngine;
using System.Collections;
 
public class FlyCamera : MonoBehaviour {

    [Header ("Mouse/WASD to move, space to ascend, left shift to speed up")]
    public bool disableUI;
    public bool disablePlayers;
    public float speed = 1.0f;
    public float ySpeedMultiplier = 0.5f;
    public float fastSpeed = 2.0f;
    public float mouseSpeed = 4.0f;
    private Vector3 _angles;
    private Canvas canvas;

    private void OnEnable() {
        _angles = transform.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() {
        if (GameObject.Find("UICanvas") != null) {
            canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        
            if (disableUI) {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }
        else
        {
            Debug.Log("Couldn't find 'UICanvas'");
        }

        if (disablePlayers) {
            GameObject.FindGameObjectWithTag("Player").SetActive(false);
            GameObject.FindGameObjectWithTag("Player2").SetActive(false);
        }
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