// Script dependencies 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component dependencies 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class CharacterBehaviour : MonoBehaviour{

    // Character Settings 
    [SerializeField]float turnSpeed_Moving = 360; // characters turn speed while moving
    [SerializeField]float turnSpeed_Idle = 180; // characters turn speed while idle
    [SerializeField] float JumpForce = 12f; // force applied when jumping
    [SerializeField] float gravityMultiplier = 2f; // gravity applied when falling for "snappy" platforming
    [SerializeField] float moveSpeedMultiplier = 1f; // speed setting
    [SerializeField] float animationSpeedMultiplyer = 1f; // corrosponding animation speed
    [SerializeField] float runCycleOffset = .2f; // Ofset for leg by leg running to translate into jumping
    [SerializeField] float groundCheckDistance = .1f; // Desired distance to check ground

    // Component refferences
    Rigidbody _Rb; // required rigidbody
    Animator _Anim; // required animator
    CapsuleCollider _CapCol; // required capsule collider
    
    // functional variables 
    Vector3 _CapColCenter; // Center of the attached capsule collider

	

    float _CapColHeight; // Height of the attached capsule collider

    bool grounded; // whether the character is grounded
    Vector3 groundAngle; // Normalized hitinfo while groundcheking (Ground angle)
    
    bool crouching; // whether the character is currently crouching

	[HideInInspector]
    public bool _InFight; // whether the character is currently in combat stance

    float turnAmount; // potential turning amount
    float forwardAmount; // forward movement speed of the character
    
    const float constantHalf = .5f; // constant for using .5f
    float initalGroundCheckDist; // initally set distance for ground check raycast
 
    
    void Start(){ // Start is called before the first frame update
        _Anim = GetComponent<Animator>(); // getting ref to animator

        _Rb = GetComponent<Rigidbody>(); // getting ref to Rigidbody

		
        
        // Presetting / ensuring Rigidbody constraints
        _Rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        _CapCol = GetComponent<CapsuleCollider>(); // getting ref to CapsuleCollider
        _CapColHeight = _CapCol.height; // storing collider height
        _CapColCenter = _CapCol.center; // storing collider origin

        initalGroundCheckDist = groundCheckDistance; // strong inital groundCheck distance
    }

    public void Move(Vector3 move, bool crouch, bool jump){ // Overall movement and input handling

		//Check infight state
		_InFight = _Anim.GetBool("InFight");

		GroundCheck(); // check whether character is grounded 

		if(!_InFight){
			// convert the world relative moveInput vector into a local-relative turn amount and forward amount required to head in the desired direction.
			if (move.magnitude > 1f) move.Normalize();
		
			move = transform.InverseTransformDirection(move); // Worldspace -> localspace
			move = Vector3.ProjectOnPlane(move, groundAngle); // move according to underlying terrain steepness

			turnAmount = Mathf.Atan2(move.x, move.z); // Calculating actual turn amount for movement
		}	else turnAmount = move.x; // else take direct -1,1 input
		
		forwardAmount = move.z; // setting forward amount to forward motion

		if(!_InFight)AdjustTurning(); // Adjusting turn speed
		
		if (grounded){ // triggering adequate moovement handler
			HandleJump(crouch, jump);  // on ground
		} else {
			HandleMidAir(); // mid air
		}

		AdjustColliderForCrouch(crouch); // scaling capsule if required
		CheckForLowArea(); // checking terrain above
		// send input and other state parameters to the animator
		HandleAnimations(move); // handeling animations
	}



    void HandleAnimations(Vector3 move){ // Handles animation, rootmotion speed and jump ik

		float _Damp = 0;

		if(!_InFight) _Damp = .1f;

		// update animator parameters
		_Anim.SetFloat("Forward", forwardAmount, _Damp, Time.deltaTime);
		_Anim.SetFloat("Turn", turnAmount, _Damp, Time.deltaTime);
		_Anim.SetBool("Crouch", crouching);
		_Anim.SetBool("OnGround", grounded);
		
        if (!grounded){
			_Anim.SetFloat("Jump", _Rb.velocity.y);
		}
		
		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// Specific to unity standart jumping and running animations
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle = Mathf.Repeat(_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleOffset, 1);

		float jumpLeg = (runCycle < constantHalf ? 1 : -1) * forwardAmount; // if runCycle is smaller than .5, jumpleg = 1, else -1
		
        if (grounded){ // if the character is on ground, adjust jumpleg animation
			_Anim.SetFloat("JumpLeg", jumpLeg);
		}

		// twraking animation speed and with this movement speed due to root motion
		if (grounded && move.magnitude > 0){ // if character is grounded and moving

			_Anim.speed = animationSpeedMultiplyer; // setting animation speed to desired multiplier

		} else { // not while mid air
			
			_Anim.speed = 1; // Animating with standart speed
		}
	}

    void AdjustColliderForCrouch(bool crouch){ // Ensuring that the capsule collider is scaled when crouching, called when toggeling crouch
			
        if (grounded && crouch){ // if the character is on ground and crouching is toggled on
			
            if (crouching) {return;} // return if the character is already crouching to prevent double rescscaling
			_CapCol.height = _CapCol.height / 2f;
			_CapCol.center = _CapCol.center / 2f;
			crouching = true;

		} else { // if crouching is toggled off

			Ray crouchRay = new Ray(_Rb.position + Vector3.up * _CapCol.radius * constantHalf, Vector3.up); // Calculating upwards ray 
			float crouchRayLength = _CapColHeight - _CapCol.radius * constantHalf; // calculating length to check above

            // if the character has to be cruched ( terrain above or similar)
			if (Physics.SphereCast(crouchRay, _CapCol.radius * constantHalf, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore)){
				crouching = true; // maintain true to crouching
				return; // exit method
			}

			_CapCol.height = _CapColHeight; // resetting collider to inital height
			_CapCol.center = _CapColCenter; // readjusting to inital origin
			crouching = false; // updating crouching bool
		}
    }

    void CheckForLowArea(){ // Prevents to stand and clip in low ceiling areas

		if (!crouching){ // if the character isn't already crouching

			Ray crouchRay = new Ray(_Rb.position + Vector3.up * _CapCol.radius * constantHalf, Vector3.up); // Calculating upwards ray to check above
			float crouchRayLength = _CapColHeight - _CapCol.radius * constantHalf; // calculating length to check above
            
            // if the character has to be cruched ( terrain above or similar)
			if (Physics.SphereCast(crouchRay, _CapCol.radius * constantHalf, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore)){
				crouching = true; // Enforce crouching
			}
		}
	}

	void HandleJump(bool crouch, bool jump){
			
		if (jump && !crouch && (_Anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") || _Anim.GetCurrentAnimatorStateInfo(0).IsName("ArmedLocomotion"))){ // Check Jump conditions
				
            // Set Rigidbody vertical velocity to jumping
			_Rb.velocity = new Vector3(_Rb.velocity.x, JumpForce, _Rb.velocity.z);
			grounded = false; // Update grounded bool
			_Anim.applyRootMotion = false; // Disable rootmotion when jumping
			groundCheckDistance = 0.1f; // reducing groundCheck distance while jumping

		}
	}

    void HandleMidAir(){ // Handle extra gravity and groundCheck distance while not grounded
		
        // apply gravity multiplier while in air
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity; // calculating current extra gravity
		_Rb.AddForce(extraGravityForce); // adding extra gravity

        // keep groundcheck distance at minimum while veolcity is below 0, else reset to inital distance
		groundCheckDistance = _Rb.velocity.y < 0 ? initalGroundCheckDist : 0.01f;
	}

    void AdjustTurning(){ // adjusting the turnspeed of the character in addition to animated root motion when moving

		float turnSpeed = Mathf.Lerp(turnSpeed_Idle, turnSpeed_Moving, forwardAmount); // Increasing turn speed towards moving turn speed
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0); // rotating the character in additon to root motion
	}

	void HandleCombatMode(bool combat, bool crouch){

		// Differentiate air and ground combat
			// set off corrosponding combat mode

	}

	void HandleGroundCombatAnimations(){

		// Determin previous action
			// Trigger corrosponding anim

	}

	void HandleAirCombatAnimations(){

		// Set off airCombat Animation
		
		//reset to idle
	}



    void GroundCheck(){ // method to check whether the character is grounded 

		RaycastHit hitInfo; // instantiating empty raycast hit info 

		// Groundcheck ray Debug
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));

		// .1f adjusting to "skinwidth" to cast slightly from the inside of the model
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance)){ // casting downards checking for ground

			groundAngle = hitInfo.normal; // angle to ground
			grounded = true; // setting grounded boolean
			_Anim.applyRootMotion = true; // applying root moting in animator if on ground

		} else{ // if character is not on ground 

			grounded = false; // setting grounded boolean
			groundAngle = Vector3.up; // ground angle is simply upwards
			_Anim.applyRootMotion = false; // preventing rootmoting during falling and jumping
		}
	}

    
    // Callback for animation movements to adjust root motion before applied
    public void OnAnimatorMove() {
        
        if (grounded && Time.deltaTime > 0){ // only adjust if character is grounded
            
            // getting the adjusted movement of the character taking animation into account
			Vector3 moveAdjustment = (_Anim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime; 
			
			moveAdjustment.y = _Rb.velocity.y; // preserving current vertical velocity
			_Rb.velocity = moveAdjustment; // Updating character velocity
		}
    }
	
}
