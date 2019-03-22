using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Slow Motion")]
    [Tooltip("HotKey to active slow motion")]
    [SerializeField]
    private KeyCode slowMoHotkey = KeyCode.Space;

    [Tooltip("HotKey to reset current scene")]
    [SerializeField]
    private KeyCode resetHotkey = KeyCode.R;

    [SerializeField]
    private bool holdHotKey = true;
    
    [SerializeField]
    private float slowMotionTimeScale = 0.4f;
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        SlowMotion();
        ResetScene();
        QuitGame();


        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(3);
        }
    }

    #region Quit Game
    private void QuitGame()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
    #endregion

    #region Reset Scene
    private void ResetScene() {
        if (Input.GetKey(resetHotkey)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion

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
