using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    #region Mouse Control
    [System.Serializable]
    public class MouseControl {
        /*  public Vector2 Damping;
        public Vector2 Sensitivity; */
        public bool hideCursor;
        public bool LockMouse;
    }
    [SerializeField] public MouseControl _MouseControl;
    #endregion

    #region Controller Control
    [System.Serializable]
    public class ControllerControl {
        public float ControllerTriggerAxisLimit = 0.5f;
    }
    [SerializeField] public ControllerControl _ControllerControl;
    #endregion

    //SET
    #region SET
    #region Player
    [System.Serializable]
    private class Player {  //MOUSE PLAYER
        [Tooltip ("Enable if controls are setup for a controller")]
        public bool useController;
        public string _Vertical = "Vertical";
        public string _Horizontal = "Horizontal";
        public string _Jump = "Jump";
        public string _MouseInputX = "Mouse X";
        public string _MouseInputY = "Mouse Y";
        public string _Fire1 = "Fire1";
        public string _Fire2 = "Fire2";
        public string _isRunning = "Run";
    }
    [SerializeField] private Player _Player;
    #endregion

    #region Player 1
    [System.Serializable]
    private class Player1 {
        [Tooltip ("Enable if controls are setup for a controller")]
        public bool useController;
        public string _Vertical = "Vertical";
        public string _Horizontal = "Horizontal";
        public string _Jump = "Jump";
        public string _MouseInputX = "Mouse X";
        public string _MouseInputY = "Mouse Y";
        public string _Fire1 = "Fire1";
        public string _Fire2 = "Fire2";
        public string _isRunning = "Run";
    }
    [SerializeField] private Player1 _Player1;
    #endregion
    
    #region Player 2
    [System.Serializable]
    private class Player2 {
        [Tooltip ("Enable if controls are setup for a controller")]
        public bool useController;
        public string _Vertical = "Vertical2";
        public string _Horizontal = "Horizontal2";
        public string _Jump = "Jump2";
        public string _MouseInputX = "MouseX2";
        public string _MouseInputY = "MouseY2";
        public string _Fire1 = "Fire12";
        public string _Fire2 = "Fire22";
        public string _isRunning = "Run2";
    }
    [SerializeField] private Player2 _Player2;
    #endregion
    #endregion

    #region GET
    #region Player
    [Header ("Player")]
    [Header ("GET Values")]
    public float Vertical;
    public float Horizontal;
    public bool Jump;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool isRunning;
    #endregion

    #region Player 1
    [Header ("Player1")]
    public float Vertical1;
    public float Horizontal1;
    public bool Jump1;
    public Vector2 MouseInput1;
    public bool Fire11;
    public bool Fire21;
    public bool isRunning1;
    #endregion

    #region Player 2
    [Header ("Player2")]
    public float Vertical2;
    public float Horizontal2;
    public bool Jump2;
    public Vector2 MouseInput2;
    public bool Fire12;
    public bool Fire22;
    public bool isRunning2;
    #endregion
    #endregion
    
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
        #region Player
        if (_Player.useController) {
            //Round controller input
            Vertical = Mathf.Round(Input.GetAxisRaw(_Player._Vertical));
            Horizontal = Mathf.Round(Input.GetAxisRaw(_Player._Horizontal));

            //Convert fire float to bool
            Fire1 = (Input.GetAxis(_Player._Fire1) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Fire2 = (Input.GetAxis(_Player._Fire2) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
        }
        else {
            Vertical = Input.GetAxisRaw(_Player._Vertical);
            Horizontal = Input.GetAxisRaw(_Player._Horizontal);

            Fire1 = Input.GetButtonDown(_Player._Fire1);
            Fire2 = Input.GetButtonDown(_Player._Fire2);
        }

        Jump = Input.GetButtonDown(_Player._Jump);
        MouseInput = new Vector2(Input.GetAxisRaw(_Player._MouseInputX), Input.GetAxisRaw(_Player._MouseInputY));
        isRunning = Input.GetButton(_Player._isRunning);
        #endregion

        #region Player1
        if (_Player1.useController) {
            //Round controller input
            Vertical1 = Mathf.Round(Input.GetAxisRaw(_Player1._Vertical));
            Horizontal1 = Mathf.Round(Input.GetAxisRaw(_Player1._Horizontal));

            //Convert fire float to bool
            Fire11 = (Input.GetAxis(_Player1._Fire1) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Fire21 = (Input.GetAxis(_Player1._Fire2) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
        }
        else {
            Vertical1 = Input.GetAxisRaw(_Player1._Vertical);
            Horizontal1 = Input.GetAxisRaw(_Player1._Horizontal);

            Fire11 = Input.GetButtonDown(_Player1._Fire1);
            Fire21 = Input.GetButtonDown(_Player1._Fire2);
        }

        Jump1 = Input.GetButtonDown(_Player1._Jump);
        MouseInput1 = new Vector2(Input.GetAxisRaw(_Player1._MouseInputX), Input.GetAxisRaw(_Player1._MouseInputY));
        isRunning1 = Input.GetButton(_Player1._isRunning);
        #endregion
        
        #region Player2
        if (_Player2.useController) {
            //Round controller input
            Vertical2 = Mathf.Round(Input.GetAxisRaw(_Player2._Vertical));
            Horizontal2 = Mathf.Round(Input.GetAxisRaw(_Player2._Horizontal));

            //Convert fire float to bool
            Fire12 = ((Input.GetAxis(_Player2._Fire1)) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Fire22 = ((Input.GetAxis(_Player2._Fire2)) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            /* Fire12 = Input.GetAxis(_Player2._Fire12);
            Fire22 = Input.GetAxis(_Player2._Fire22); */
        }
        else
        {
            Vertical2 = Input.GetAxisRaw(_Player2._Vertical);
            Horizontal2 = Input.GetAxisRaw(_Player2._Horizontal);

            Fire12 = Input.GetButtonDown(_Player2._Fire1);
            Fire22 = Input.GetButtonDown(_Player2._Fire2);
        }

        Jump2 = Input.GetButtonDown(_Player2._Jump);
        MouseInput2 = new Vector2(Input.GetAxisRaw(_Player2._MouseInputX), Input.GetAxisRaw(_Player2._MouseInputY));
        isRunning2 = Input.GetButton(_Player2._isRunning);
        #endregion
    }
}
