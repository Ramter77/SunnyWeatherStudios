using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers
{
    public event System.Action<PlayerCont> OnLocalPlayerJoined;

    private GameObject gameObject;
    private static GameManagers m_Instance;
    public static GameManagers Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new GameManagers();
                m_Instance.gameObject = new GameObject("_gameManager");
            }
            return m_Instance;
        }
    }

    private InputManager m_InputManager;
    public InputManager InputManager {
        get {
            if (m_InputManager == null) {
                m_InputManager = gameObject.AddComponent<InputManager>();
            }
            return m_InputManager;
        }
    }

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
}
