using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonCam : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float damping;

    Transform cameraLookTarget;
    PlayerCont localPlayer;

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
        Vector3 targetPos = cameraLookTarget.position + localPlayer.transform.forward * cameraOffset.z + 
                                                        localPlayer.transform.up * cameraOffset.y +
                                                        localPlayer.transform.right * cameraOffset.x;
        Quaternion targetRot = Quaternion.LookRotation(cameraLookTarget.position - targetPos, Vector3.up);


        transform.position = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, damping * Time.deltaTime);
    }
}
