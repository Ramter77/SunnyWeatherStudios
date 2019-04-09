using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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
