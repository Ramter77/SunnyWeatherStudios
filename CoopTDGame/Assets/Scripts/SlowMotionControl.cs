using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionControl : MonoBehaviour
{
    #region Variables
    [Header("Slow Motion")]
    [Tooltip("HotKey to activate slow motion")]
    [SerializeField]
    private KeyCode slowMoHotkey = KeyCode.Space;

    [SerializeField]
    private bool holdHotKey = true;
    
    [SerializeField]
    private float slowMotionTimeScale = 0.4f;
    #endregion

    void Update() {
        SlowMotion();
    }

    #region SlowMotion
    private void SlowMotion()
    {
        if (holdHotKey) 
        {
            if (Input.GetKey(slowMoHotkey)) 
            {
                Time.timeScale = slowMotionTimeScale;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            if (Input.GetKeyDown(slowMoHotkey)) {
                if (Time.timeScale == 1.0f)
                    Time.timeScale = slowMotionTimeScale;
                else
                    Time.timeScale = 1.0f;
                // Adjust fixed delta time according to timescale
                // The fixed delta time will now be 0.02 frames per real-time second
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }        
    }
    #endregion
}
