using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] public Vector3 cameraOffset;
    [SerializeField] float damping;

    Transform cameraLookTarget;
    PlayerCont localPlayer;
    private float mouseInputY;
    private PlayerCont.MouseInput _mouseControl;
    private float _yDamping, _ySensitivity, _minAngle, _maxAngle;
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

        _mouseControl = player.MouseControl;
        _yDamping = _mouseControl.Damping.y;
        _ySensitivity = _mouseControl.Sensitivity.y;
        _minAngle = _mouseControl.minAngle;
        _maxAngle = _mouseControl.maxAngle;
    }

    void Update() {
        if (localPlayer == null) {
            localPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCont>();
        }

        if (cameraLookTarget == null) {
            cameraLookTarget = localPlayer.transform.Find("cameraLookTarget");
        }
        

        #region Y Input
        mouseInputY -= Mathf.Lerp(mouseInputY, GameManagers.Instance.InputManager.MouseInput.y, 1f / _yDamping);

        if (mouseInputY > _minAngle && mouseInputY < _maxAngle) {
            cameraOffset.y = localPlayer.transform.up.y * mouseInputY * _ySensitivity;
        } 
        #endregion


        Vector3 targetPos = cameraLookTarget.position + localPlayer.transform.forward * cameraOffset.z + 
                                                        localPlayer.transform.up      * cameraOffset.y +
                                                        localPlayer.transform.right   * cameraOffset.x;
        Quaternion targetRot = Quaternion.LookRotation(cameraLookTarget.position - targetPos, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, damping * Time.deltaTime);
    }
}
