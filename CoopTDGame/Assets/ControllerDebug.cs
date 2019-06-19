using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDebug : MonoBehaviour
{
    private int frames = 0;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Log();
    }

    void Update()
    {
        frames++;
        if (frames % 6000 == 0) {
            Log();
        }
    }

    void Log() {
        //Debug.Log("---------CONTROLS---------");
        //Debug.Log("Mouse present: " + Input.mousePresent);
        Debug.Log("Controllers: ");
        string[] JoystickNames = Input.GetJoystickNames();
        
        foreach (string str in JoystickNames) {
            Debug.Log(str);
        }
        
        //Debug.Log("--------------------------");
    }
}
