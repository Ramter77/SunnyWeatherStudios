using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ControllerDebug : MonoBehaviour
{
    private int frames = 0;

    void Update()
    {
        frames++;
        if (frames % 3600 == 0) {
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
}
