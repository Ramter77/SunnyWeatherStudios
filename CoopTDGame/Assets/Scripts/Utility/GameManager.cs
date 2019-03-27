using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    #region OnLocalPlayerJoined
    public event System.Action<PlayerCont> OnLocalPlayerJoined;

    private PlayerCont m_LocalPlayer;
    public PlayerCont LocalPlayer {
        get {
            return m_LocalPlayer;
        }
        set {
            m_LocalPlayer = value;
            if (OnLocalPlayerJoined != null) {
                OnLocalPlayerJoined(m_LocalPlayer);
            }
        }
    }
    #endregion
}
