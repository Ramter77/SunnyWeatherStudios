﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePrefabCollisionColor : MonoBehaviour
{
    [Tooltip("The layer in the surface manager that the prefabs cannot be placed on.")]
    [SerializeField] int pathLayer1 = 4;
    [SerializeField] int pathLayer2 = 3;

    [Tooltip("If this is enabled, you can see how far the script will check for ground, and the radius of the check.")]
	[SerializeField] bool debugMode = true;
    
    #region Ground check
    [Tooltip("How high, relative to the character's pivot point the start of the ray is.")]
	[SerializeField] float groundCheckHeight = 0.5f;
    [Tooltip("What is the radius of the ray.")]
	[SerializeField] float groundCheckRadius = 5f;
    [Tooltip("How far the ray is casted.")]
	[SerializeField] float groundCheckDistance = 1f;

    [Tooltip("What are the layers that should be taken into account when checking for ground.")]
	[SerializeField] LayerMask groundLayers = 10;
    #endregion


    private PlacePrefab _PlacePrefabScript;
    private RaycastHit currentGroundInfo;
    private bool isGrounded;
    private bool isColliding;
    private bool isColliding1;

    public int placeByPlayer_;

    void Start()
    {
        //_PlacePrefabScript = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlacePrefab>();
    }



    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void LateUpdate()
    {
        CheckGround();

        if (placeByPlayer_ == 1) {
            _PlacePrefabScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlacePrefab>();
        }
        else {
            _PlacePrefabScript = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlacePrefab>();
        }

        //if (isColliding) {
            _PlacePrefabScript.SetColorToRed(true);
            //Debug.Log("SETIING SREDD");
        //}
        if (!isColliding) {
            _PlacePrefabScript.SetColorToRed(false);
        }
    }

    void CheckGround() {
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckHeight, Vector3.down);

        if (Physics.SphereCast(ray, groundCheckRadius, out currentGroundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore)) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }

        //If grounded get the current terrain texture index
        if (isGrounded) { GetTerrainSurfaceIndex(); }
    }

    void GetTerrainSurfaceIndex() {
        //Get current terrain texture index from the SurfaceManager
        int surfaceIndex = SurfaceManager.singleton.GetSurfaceIndex(currentGroundInfo.collider, currentGroundInfo.point);

        if (debugMode) {
            //Debug.Log("Current surfaceIndex " + surfaceIndex);
        }

        //Set color accordingly
        if (surfaceIndex == pathLayer1 || surfaceIndex == pathLayer2) {
            isColliding = true;
        }
        else {
            //if (!isColliding) {
                //_PlacePrefabScript.SetColorToRed(false);
            //}
            if (!isColliding1) {
                isColliding = false;
            }
        }
    }

    #region Ground check Gizmo
    void OnDrawGizmos() {
        if(debugMode) {
            Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
        }
    }
    #endregion



    #region OnTriggerStay (Change color of material to red on collision (with non-Terrain)
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Terrain") {
            //Debug.Log("SetColorToRed() RED: " + other.name);
            //isColliding1 = true;

            isColliding = true;
            isColliding1 = true;
        }
    }
    #endregion

    /* void OnTriggerStay(Collider other)
    {
        if (other.name != "Terrain") {
            //Debug.Log("SetColorToRed(): " + other.name);
            isColliding = true;
            isColliding1 = true;
        }
    } */

    #region OnTriggerStay (Change color of material to red on collision (with non-Terrain)
    void OnTriggerExit(Collider other)
    {
        if (other.name != "Terrain") {
            //Debug.Log("SetColorToRed(): " + other.name);
            //isColliding1 = false;

            isColliding = false;
            isColliding1 = false;
        }
    }
    #endregion

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        
    }
}
