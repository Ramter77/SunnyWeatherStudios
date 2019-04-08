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
        public string _Heal = "Heal";
        public string _Slash = "Slash";
        public string _Ultimate = "Ultimate";
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
        public string _Jump = "Jump1";
        public string _MouseInputX = "Mouse X";
        public string _MouseInputY = "Mouse Y";
        public string _Fire1 = "Fire1";
        public string _Fire2 = "Fire2";
        public string _isRunning = "Run";
        public string _Heal = "Heal1";
        public string _Slash = "Slash1";
        public string _Ultimate = "Ultimate1";
        public string _BuildMode = "BuildMode1";
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
        public string _Heal = "Heal2";
        public string _Slash = "Slash2";
        public string _Ultimate = "Ultimate2";
        public string _BuildMode = "BuildMode2";
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
    public bool Heal;
    public bool Slash;
    public bool Ultimate;
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
    public bool Heal1;
    public bool Slash1;
    public bool Ultimate1;
    public bool BuildMode1;
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
    public bool Heal2;
    public bool Slash2;
    public bool Ultimate2;
    public bool BuildMode2;




     public string        _strJoystickName;
         public int            _iButtonNumber;            // 0 indexed
         public int            _iJoystickNumber;        // 1 indexed
         public string        _strButtonIdentifier;  
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

    /* public bool ButtonIsPressed()
         {
             // N.B. you could use Input.KeyDown( KeyCode ) if you parse the string 
             // into a value of the KeyCode enum http://stackoverflow.com/a/16104
             return Input.GetButtonDown( _strButtonIdentifier );
         }

         public void InitialiseButtonIdentifier( string strJoystickName, int iButtonNumber )
         {
             _strJoystickName    = strJoystickName;
             _iButtonNumber        = iButtonNumber;
 
             string[] astrJoysticks = Input.GetJoystickNames();
 
             // find the named joystick in the array
             // array position corresponds to joystick number
             for( int i = 0; i < astrJoysticks.Length; ++ i )
             {
                 if( _strJoystickName == astrJoysticks[ i ] )
                 {
                     _iJoystickNumber = ( i + 1 );
                     _strButtonIdentifier = string.Format(    "joystick{0}button{1}", 
                                                             _iJoystickNumber, 
                                                             _iButtonNumber );
                 }
             }
         } */
     

    void Update()
    {
        /* for (int i = 0; i < 3; i++)
        {
        //Debug.Log(Input.GetJoystickNames()[i] + " is moved");

        string[] astrJoysticks = Input.GetJoystickNames();

        // find the named joystick in the array
             // array position corresponds to joystick number
             for( int i = 0; i < astrJoysticks.Length; ++ i )
             {
                 if( _strJoystickName == astrJoysticks[ i ] )
                 {
                     _iJoystickNumber = ( i + 1 );
                     _strButtonIdentifier = string.Format(    "joystick{0}button{1}", 
                                                             _iJoystickNumber, 
                                                             _iButtonNumber );
                 }
             }
        } */

         
 
         
 
         


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
        Heal = Input.GetButtonDown(_Player._Heal);
        Slash = Input.GetButtonDown(_Player._Slash);
        Ultimate = Input.GetButtonDown(_Player._Ultimate);
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
        //Debug.Log(Jump1 + " JUas ckmaf");
        MouseInput1 = new Vector2(Input.GetAxisRaw(_Player1._MouseInputX), Input.GetAxisRaw(_Player1._MouseInputY));
        isRunning1 = Input.GetButton(_Player1._isRunning);
        Heal1 = Input.GetButtonDown(_Player1._Heal);
        Slash1 = Input.GetButtonDown(_Player1._Slash);
        Ultimate1 = Input.GetButtonDown(_Player1._Ultimate);
        BuildMode1 = Input.GetButton(_Player1._BuildMode);
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
        Heal2 = Input.GetButtonDown(_Player2._Heal);
        Slash2 = Input.GetButtonDown(_Player2._Slash);
        Ultimate2 = Input.GetButtonDown(_Player2._Ultimate);
        BuildMode2 = Input.GetButton(_Player2._BuildMode);



        //Debug.Log("BM: "+BuildMode1 + " asfaefkn BM2: "+BuildMode2);
        #endregion
    }
}
