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

    public LayerMask layerMask;

    private Camera cam;
    Vector3 desiredCameraPos;

    void Start()
    {
        //dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;

        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.25F, 0.5F, 0));
        RaycastHit hit;
        

        Vector3 desiredCameraPos = transform.parent.transform.parent.TransformPoint(dollyDir * maxDistance);//dollyDir * maxDistance;//transform.parent.position;//(dollyDir * maxDistance);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, layerMask)) {
        //if(Physics.Linecast(desiredCameraPos, transform.parent.transform.parent.transform.parent.transform.parent.transform.position, out hit, layerMask))
        //if (Physics.Raycast(desiredCameraPos, Vector3.back+3 * 10f, out hit, layerMask))

        //if (Physics.Linecast(transform.parent.transform.parent.transform.parent.position, desiredCameraPos, out hit, layerMask))
        if (Physics.Linecast(transform.parent.transform.parent.transform.parent.position, desiredCameraPos, out hit, layerMask))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
            isHitting = true;

            cam.useOcclusionCulling = false;
        }
        else
        {
            distance = maxDistance;
            //isHitting = false;
            
            cam.useOcclusionCulling = true;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }

    private void OnDrawGizmos() {
        Debug.DrawLine(transform.parent.transform.parent.position, desiredCameraPos, Color.red, 1f);
        //Debug.DrawLine(desiredCameraPos, transform.parent.transform.parent.transform.parent.transform.parent.transform.position, Color.red, 1f);
        //Debug.DrawRay(transform.position, Vector3.back * 10f, Color.red, 1f);
    }
}
