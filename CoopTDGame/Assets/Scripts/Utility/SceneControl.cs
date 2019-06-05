using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : Singleton<SceneControl>
{
    #region Variables
    [Tooltip("HotKey to reset current scene")]
    [SerializeField]
    private KeyCode resetHotkey = KeyCode.P;
    public GameObject MainMenu;
    #endregion

    void Update()
    {
        ResetScene();
        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            MainMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.01f;
        }*/


            #region DEBUG switch scene with buttons
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
        #endregion
    }



    #region Reset Scene
    private void ResetScene() {
        if (Input.GetKey(resetHotkey)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion
}
