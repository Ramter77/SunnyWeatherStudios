using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1f;
    public float maxDistance = 4f;
    public float smooth = 10f;

    public Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    public bool isHitting = false;

    private Camera cam;

    void Start()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;

        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;
        if(Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
            isHitting = true;

            cam.useOcclusionCulling = false;
        }
        else
        {
            distance = maxDistance;
            
            cam.useOcclusionCulling = true;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
