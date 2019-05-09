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
        [Tooltip ("Threshold to convert axis input to true")]
        public float ControllerTriggerAxisLimit = 0.5f;
    }
    [SerializeField] public ControllerControl _ControllerControl;
    #endregion

    //SET
    #region SET
    #region Player
    [System.Serializable]
    private class Player0 {  //MOUSE PLAYER
        [Tooltip ("Enable if controls are setup for a controller")]
        public bool useController;
        public string _Vertical = "Vertical0";
        public string _Horizontal = "Horizontal0";
        public string _MouseInputX = "MouseX0";
        public string _MouseInputY = "MouseY0";
        public string _Ranged = "Fire10";
        public string _Melee = "Fire20";
        public string _Run = "Run0";
        public string _Jump = "Jump0";
        public string _Heal = "Ability10";
        public string _Ultimate = "Ability20";
        public string _Interact = "Ability30";
        public string _LB = "LB0";
        public string _RB = "RB0";
    }
    [SerializeField] private Player0 _Player0;
    #endregion

    #region Player 1
    [System.Serializable]
    private class Player1 {
        [Tooltip ("Enable if controls are setup for a controller")]
        public bool useController;
        public string _Vertical = "Vertical1";
        public string _Horizontal = "Horizontal1";
        public string _MouseInputX = "MouseX1";
        public string _MouseInputY = "MouseY1";
        public string _Ranged = "Fire11";
        public string _Melee = "Fire21";
        public string _Run = "Run1";
        public string _Jump = "Jump1";
        public string _Heal = "Ability11";
        public string _Ultimate = "Ability21";
        public string _Interact = "Ability31";
        public string _LB = "LB1";
        public string _RB = "RB1";       
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
        public string _MouseInputX = "MouseX2";
        public string _MouseInputY = "MouseY2";
        public string _Ranged = "Fire12";
        public string _Melee = "Fire22";
        public string _Run = "Run2";
        public string _Jump = "Jump2";
        public string _Heal = "Ability12";
        public string _Ultimate = "Ability22";
        public string _Interact = "Ability32";
        public string _LB = "LB2";
        public string _RB = "RB2";
        
    }
    [SerializeField] private Player2 _Player2;
    #endregion
    #endregion

    #region GET
    #region Player
    [Header ("Player")]
    [Header ("GET Values")]
    public float Vertical0;
    public float Horizontal0;
    public Vector2 MouseInput0;
    public bool Ranged0;
    public bool Melee0;
    public bool Run0;
    public bool Jump0;
    public bool Heal0;
    public bool Ultimate0;
    public bool Interact0;
    public bool LB0;
    public bool RB0;
    #endregion

    #region Player 1
    [Header ("Player1")]
    public float Vertical1;
    public float Horizontal1;
    public Vector2 MouseInput1;
    public bool Ranged1;
    public bool Melee1;
    public bool Run1;
    public bool Jump1;
    public bool Heal1;
    public bool Ultimate1;
    public bool Interact1;
    public bool LB1;
    public bool RB1;
    #endregion

    #region Player 2
    [Header ("Player2")]
    public float Vertical2;
    public float Horizontal2;
    public Vector2 MouseInput2;
    public bool Ranged2;
    public bool Melee2;
    public bool Run2;
    public bool Jump2;
    public bool Heal2;
    public bool Ultimate2;
    public bool Interact2;
    public bool LB2;
    public bool RB2;
    




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

         
 
         
 
         


        #region Player0
        if (_Player0.useController) {
            //Round controller input
            Vertical0 = Mathf.Round(Input.GetAxisRaw(_Player0._Vertical));
            Horizontal0 = Mathf.Round(Input.GetAxisRaw(_Player0._Horizontal));

            //Convert fire float to bool
            Ranged0 = (Input.GetAxisRaw(_Player0._Ranged) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Melee0 = (Input.GetAxisRaw(_Player0._Melee) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
        }
        else {
            Vertical0 = Input.GetAxisRaw(_Player0._Vertical);
            Horizontal0 = Input.GetAxisRaw(_Player0._Horizontal);

            Ranged0 = Input.GetButton(_Player0._Ranged);
            Melee0 = Input.GetButton(_Player0._Melee);
        }

        MouseInput0 = new Vector2(Input.GetAxisRaw(_Player0._MouseInputX), Input.GetAxisRaw(_Player0._MouseInputY));
        Run0 = Input.GetButton(_Player0._Run);
        Jump0 = Input.GetButtonDown(_Player0._Jump);
        Heal0 = Input.GetButtonDown(_Player0._Heal);
        Ultimate0 = Input.GetButtonDown(_Player0._Ultimate);
        Interact0 = Input.GetButtonDown(_Player0._Interact);
        LB0 = Input.GetButtonDown(_Player0._LB);
        RB0 = Input.GetButtonDown(_Player0._RB);
        #endregion

        #region Player1
        if (_Player1.useController) {
            //Round controller input
            Vertical1 = Mathf.Round(Input.GetAxisRaw(_Player1._Vertical));
            Horizontal1 = Mathf.Round(Input.GetAxisRaw(_Player1._Horizontal));

            //Convert fire float to bool
            Ranged1 = (Input.GetAxisRaw(_Player1._Ranged) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Melee1 = (Input.GetAxisRaw(_Player1._Melee) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
        }
        else {
            Vertical1 = Input.GetAxisRaw(_Player1._Vertical);
            Horizontal1 = Input.GetAxisRaw(_Player1._Horizontal);

            Ranged1 = Input.GetButton(_Player1._Ranged);
            Melee1 = Input.GetButton(_Player1._Melee);
        }

        Jump1 = Input.GetButtonDown(_Player1._Jump);
        MouseInput1 = new Vector2(Input.GetAxisRaw(_Player1._MouseInputX), Input.GetAxisRaw(_Player1._MouseInputY));
        Run1 = Input.GetButton(_Player1._Run);
        Heal1 = Input.GetButtonDown(_Player1._Heal);
        Ultimate1 = Input.GetButtonDown(_Player1._Ultimate);
        Interact1 = Input.GetButtonDown(_Player1._Interact);
        LB1 = Input.GetButtonDown(_Player1._LB);
        RB1 = Input.GetButtonDown(_Player1._RB);
        #endregion
        
        #region Player2
        if (_Player2.useController) {
            //Round controller input
            Vertical2 = Mathf.Round(Input.GetAxisRaw(_Player2._Vertical));
            Horizontal2 = Mathf.Round(Input.GetAxisRaw(_Player2._Horizontal));

            //Convert fire float to bool
            Ranged2 = ((Input.GetAxisRaw(_Player2._Ranged)) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            Melee2 = ((Input.GetAxisRaw(_Player2._Melee)) > _ControllerControl.ControllerTriggerAxisLimit) ? true : false;
            /* Fire12 = Input.GetAxis(_Player2._Ranged2);
            Fire22 = Input.GetAxis(_Player2._Melee2); */
        }
        else
        {
            Vertical2 = Input.GetAxisRaw(_Player2._Vertical);
            Horizontal2 = Input.GetAxisRaw(_Player2._Horizontal);

            Ranged2 = Input.GetButton(_Player2._Ranged);
            Melee2 = Input.GetButton(_Player2._Melee);
        }

        Jump2 = Input.GetButtonDown(_Player2._Jump);
        MouseInput2 = new Vector2(Input.GetAxisRaw(_Player2._MouseInputX), Input.GetAxisRaw(_Player2._MouseInputY));
        Run2 = Input.GetButton(_Player2._Run);
        Heal2 = Input.GetButtonDown(_Player2._Heal);
        Ultimate2 = Input.GetButtonDown(_Player2._Ultimate);
        Interact2 = Input.GetButtonDown(_Player2._Interact);
        LB2 = Input.GetButtonDown(_Player2._LB);
        RB2 = Input.GetButtonDown(_Player2._RB);
        #endregion
    }
}
