using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject UIcanvas;
    public GameObject MainMenu;
    public GameObject menuCam;
    public GameObject player1, player2;
    private bool menuEnabled;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuEnabled) {
                menuEnabled = false;
                MainMenu.SetActive(false);
                menuCam.SetActive(false);
                player1.SetActive(true);
                player2.SetActive(true);

                MainMenu.transform.parent.GetComponent<MainMenu>().gameStarted = true;

                InputManager.Instance._MouseControl.LockMouse = false;
                InputManager.Instance._MouseControl.hideCursor = false;
                Time.timeScale = 1f;
            }
            else
            {
                menuEnabled = true;
                player1.SetActive(false);
                player2.SetActive(false);
                menuCam.SetActive(true);
                MainMenu.SetActive(true);
                
                MainMenu.transform.parent.GetComponent<MainMenu>().gameStarted = false;
            
                InputManager.Instance._MouseControl.LockMouse = true;
                InputManager.Instance._MouseControl.hideCursor = true;
                //Time.timeScale = 0.01f;
            }
        }
    }

    private void Awake() {
        //DontDestroyOnLoad(this.gameObject);
    }

    #region OnLocalPlayerJoined
    //public event System.Action<PlayerCont> OnLocalPlayerJoined;

    private PlayerController m_LocalPlayer;
    public PlayerController LocalPlayer {
        get {
            return m_LocalPlayer;
        }
        set {
            m_LocalPlayer = value;
            /* if (OnLocalPlayerJoined != null) {
                //OnLocalPlayerJoined(m_LocalPlayer);
            } */
        }
    }
    #endregion
}
