using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    
    public float Vertical;
    public float Horizontal;
    public bool Jump;
    public Vector2 MouseInput;
    public bool Fire1;
    //public bool isWalking;
    public bool isRunning;
    
/*
    [System.Serializable]
    public class WeaponData {
        public static float Vertical;
        public static float Horizontal;
        public static bool Jump;
        public static Vector2 MouseInput;
        public static bool Fire1;
        //public bool isWalking;
        public static bool isRunning;
    }
*/


    /*
    #region Player2
    public float Vertical2;
    public float Horizontal2;
    public bool Jump2;
    //public Vector2 MouseInput;
    public bool Fire12;
    //public bool isWalking;
    public bool isRunning2;
    #endregion
    */
    

    void Update()
    {
        #region Player1
        Vertical = Input.GetAxisRaw("Vertical");
        Horizontal = Input.GetAxisRaw("Horizontal");
        Jump = Input.GetButtonDown("Jump");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButtonDown("Fire1");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        #endregion

        /*
        #region Player2
        Vertical2 = Input.GetAxis("Vertical2");
        Horizontal2 = Input.GetAxis("Horizontal2");
        Jump2 = Input.GetButtonDown("Jump2");
        Fire12 = Input.GetButtonDown("Fire12");
        isRunning2 = Input.GetKey(KeyCode.LeftShift);
        #endregion
        */
    }
}
