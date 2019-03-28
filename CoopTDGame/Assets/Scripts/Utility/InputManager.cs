using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    
    [Header ("Player1")]
    [Header ("SET VALUES")]
    public string _Vertical = "Vertical";
    public string _Horizontal = "Horizontal";
    public string _Jump = "Jump";
    public string _MouseInputX = "Mouse X";
    public string _MouseInputY = "Mouse Y";
    public string _Fire1 = "Fire1";
    public string _Fire2 = "Fire2";
    public string _isRunning = "Run";
    
    [Header ("Player2")]
    public string _Vertical2 = "Vertical2";
    public string _Horizontal2 = "Horizontal2";
    public string _Jump2 = "Jump2";
    public string _MouseInputX2 = "MouseX2";
    public string _MouseInputY2 = "MouseY2";
    public string _Fire12 = "Fire12";
    public string _Fire22 = "Fire22";
    public string _isRunning2 = "Run2";

    [Header ("Player1")]
    [Header ("GET VALUES")]
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

    void Update()
    {
        #region Player1
        Vertical = Input.GetAxisRaw("Vertical");
        Horizontal = Input.GetAxisRaw("Horizontal");
        Jump = Input.GetButtonDown("Jump");
        MouseInput = new Vector2(Input.GetAxisRaw(_MouseInputX), Input.GetAxisRaw(_MouseInputY));
        Fire1 = Input.GetButtonDown("Fire1");
        Fire2 = Input.GetButtonDown("Fire2");
        isRunning = Input.GetButton("Run");
        #endregion
        
        #region Player2
        Vertical2 = Input.GetAxis("Vertical2");
        Horizontal2 = Input.GetAxis("Horizontal2");
        Jump2 = Input.GetButtonDown("Jump2");
        MouseInput2 = new Vector2(Input.GetAxisRaw(_MouseInputX2), Input.GetAxisRaw(_MouseInputY2));
        Fire12 = Input.GetAxis("Fire12");
        Fire22 = Input.GetAxis("Fire22");
        isRunning2 = Input.GetButton("Run2");
        #endregion
    }
}
