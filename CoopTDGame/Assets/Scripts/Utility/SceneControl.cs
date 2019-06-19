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
