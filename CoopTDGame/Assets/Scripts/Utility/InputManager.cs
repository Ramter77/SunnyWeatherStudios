using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [System.Serializable]
    public class MouseControl {
        /*  public Vector2 Damping;
        public Vector2 Sensitivity; */
        public bool hideCursor;
        public bool LockMouse;
    }
    [SerializeField] public MouseControl _MouseControl;

    //SET
    [System.Serializable]
    public class Player1 {
        public string _Vertical = "Vertical";
        public string _Horizontal = "Horizontal";
        public string _Jump = "Jump";
        public string _MouseInputX = "Mouse X";
        public string _MouseInputY = "Mouse Y";
        public string _Fire1 = "Fire1";
        public string _Fire2 = "Fire2";
        public string _isRunning = "Run";
    }
    [SerializeField] public Player1 _Player1;
    
    [System.Serializable]
    public class Player2 {
        public string _Vertical2 = "Vertical2";
        public string _Horizontal2 = "Horizontal2";
        public string _Jump2 = "Jump2";
        public string _MouseInputX2 = "MouseX2";
        public string _MouseInputY2 = "MouseY2";
        public string _Fire12 = "Fire12";
        public string _Fire22 = "Fire22";
        public string _isRunning2 = "Run2";
    }
    [SerializeField] public Player2 _Player2;

    [Header ("Player1")]
    [Header ("GET Values")]
    public float Vertical;
    public float Horizontal;
    public bool Jump;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool isRunning;

    [Header ("Player2")]
    public float Vertical2;
    public float Horizontal2;
    public bool Jump2;
    public Vector2 MouseInput2;
    public float Fire12;
    public float Fire22;
    public bool isRunning2;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        #region Lock Mouse & Hide Cursor
        if (_MouseControl.hideCursor) {
            Cursor.visible = false;
        }
        else {
            Cursor.visible = true;
        }

        if (_MouseControl.LockMouse) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else { 
            Cursor.lockState = CursorLockMode.None;
        }
        #endregion
    }

    void Update()
    {
        #region Player1
        Vertical = Input.GetAxisRaw(_Player1._Vertical);
        Horizontal = Input.GetAxisRaw(_Player1._Horizontal);
        Jump = Input.GetButtonDown(_Player1._Jump);
        MouseInput = new Vector2(Input.GetAxisRaw(_Player1._MouseInputX), Input.GetAxisRaw(_Player1._MouseInputY));
        Fire1 = Input.GetButtonDown(_Player1._Fire1);
        Fire2 = Input.GetButtonDown(_Player1._Fire2);
        isRunning = Input.GetButton(_Player1._isRunning);
        #endregion
        
        #region Player2
        Vertical2 = Input.GetAxis(_Player2._Vertical2);
        Horizontal2 = Input.GetAxis(_Player2._Horizontal2);
        Jump2 = Input.GetButtonDown(_Player2._Jump2);
        MouseInput2 = new Vector2(Input.GetAxisRaw(_Player2._MouseInputX2), Input.GetAxisRaw(_Player2._MouseInputY2));
        Fire12 = Input.GetAxis(_Player2._Fire12);
        Fire22 = Input.GetAxis(_Player2._Fire22);
        isRunning2 = Input.GetButton(_Player2._isRunning2);
        #endregion
    }
}
