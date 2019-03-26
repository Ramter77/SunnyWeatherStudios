using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float damping;

    Transform cameraLookTarget;
    PlayerCont localPlayer;
    private float mouseInputY;
    private float yDamping = 1;

    void Awake()
    {
        GameManagers.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    void HandleOnLocalPlayerJoined(PlayerCont player) {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform.Find("cameraLookTarget");

        if (cameraLookTarget == null) {
            Debug.Log("cameraLookTarget == null");
            cameraLookTarget = localPlayer.transform;
        }
    }

    void Update() {
        if (localPlayer == null) {
            localPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCont>();
        }

        if (cameraLookTarget == null) {
            cameraLookTarget = localPlayer.transform.Find("cameraLookTarget");
        }

        //mouseInputY = Mathf.Lerp(mouseInputY, GameManagers.Instance.InputManager.MouseInput.y, 1f / yDamping);

        mouseInputY -= GameManagers.Instance.InputManager.MouseInput.y;

        if (mouseInputY > 1 && mouseInputY < 10) {
            cameraOffset.y = mouseInputY;
        }    


        Vector3 targetPos = cameraLookTarget.position + localPlayer.transform.forward * cameraOffset.z + 
                                                        localPlayer.transform.up      * cameraOffset.y +
                                                        localPlayer.transform.right   * cameraOffset.x;
        Quaternion targetRot = Quaternion.LookRotation(cameraLookTarget.position - targetPos, Vector3.up);


        transform.position = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, damping * Time.deltaTime);
    }
}
