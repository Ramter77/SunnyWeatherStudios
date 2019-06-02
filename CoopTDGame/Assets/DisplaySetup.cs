using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySetup : MonoBehaviour
{
    public bool use2Displays = false;
    public bool enableFlyCam = false;
    public GameObject FlyCam;

    // Use this for initialization
    void Start()
    {
        if (use2Displays)
        {
            Debug.Log("displays connected: " + Display.displays.Length);
            // Display.displays[0] is the primary, default display and is always ON.
            // Check if additional displays are available and activate each.
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
            if (Display.displays.Length > 2)
                Display.displays[2].Activate();
        }

        if (enableFlyCam)
        {
            if (FlyCam != null)
            {
                FlyCam.SetActive(true);
            }
        }
    }
}
