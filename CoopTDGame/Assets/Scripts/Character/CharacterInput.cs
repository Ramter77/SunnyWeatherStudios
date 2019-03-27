// Script Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component dependencies
[RequireComponent(typeof (CharacterBehaviour))]
[RequireComponent(typeof (CharacterCombat))]
public class CharacterInput : MonoBehaviour{

    private CharacterBehaviour _Char;    // ref to behaviour script
    private CharacterCombat _Combat;    // ref to combat handling script

    private Transform mainCam;          // ref to main camera transform for cam based movement controlls
    private Vector3 mainCamForward;     // current forward vector of mainCamera
    private Vector3 move;               // movedirection, taken from player input and camera
    private bool jump;                  // whether to jump or not

    

        
    private void Start(){

        Cursor.lockState = CursorLockMode.Locked;
       
        if (Camera.main != null){ // if there is a "main camera" tagged camera 
            mainCam = Camera.main.transform; // getting reference to it's transform 
        } else { // log error
            Debug.LogWarning( "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // use self-relative controlls instead
        }    
        _Char = GetComponent<CharacterBehaviour>(); // getting ref to behviour script

        _Combat = GetComponent<CharacterCombat>(); // getting ref to combat script
    }

    private void Update(){
        
        if (!jump){ // if not already jumping
            jump = Input.GetButtonDown("Jump");
            
        }

        if(Input.GetMouseButtonDown(0))_Combat.Attack(1);
        if(Input.GetMouseButtonDown(1))_Combat.Attack(2);
        if(Input.GetKeyDown(KeyCode.X))_Combat.SwitchStance(true);
       

        _Combat.Block(Input.GetKey(KeyCode.LeftControl));


        

    }


    // Fixed update is called in sync with physics
    private void FixedUpdate(){
            
        // get Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction 
        if (mainCam != null && !_Char._InFight) { // if main camera was found 
            
            // calculate camera relative direction to move:
            mainCamForward = Vector3.Scale(mainCam.forward, new Vector3(1, 0, 1)).normalized;
            move = vertical*mainCamForward + horizontal*mainCam.right;

        } else {
            // use world-relative directions 
            move = new Vector3(horizontal, 0, vertical)/* vertical*Vector3.forward + horizontal*Vector3.right */;
        }

	    if (Input.GetKey(KeyCode.LeftShift)) {move *= 0.5f;} // walk speed multiplier

        //if(jump)Debug.Log("jump");

        // pass input to behaviour
        _Char.Move(move, crouch, jump);
        jump = false; // reset jump input

    }
}

