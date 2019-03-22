using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePrefabCollisionColor : MonoBehaviour
{
    private PlacePrefab _PlacePrefabScript;

    void Start()
    {
        _PlacePrefabScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlacePrefab>();
    }

    #region OnTriggerStay (Change color of material to red on collision (with non-Terrain)
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Terrain") {
            //Debug.Log("SetColorToRed(): " + other.name);
            _PlacePrefabScript.SetColorToRed(true);
        }
    }
    #endregion

    #region OnTriggerStay (Change color of material to red on collision (with non-Terrain)
    void OnTriggerExit(Collider other)
    {
        if (other.name != "Terrain") {
            //Debug.Log("SetColorToRed(): " + other.name);
            _PlacePrefabScript.SetColorToRed(false);
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
