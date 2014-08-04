using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Final_ThirdPersonController : MonoBehaviour {
	
	enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
	}
	
	private CharacterState _characterState;
	
	// The speed when walking
	public float walkSpeed= 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed= 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed= 6.0f;
	
	public float inAirControlAcceleration= 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight= 0.5f;
	
	// The gravity for the character
	public float gravity= 20.0f;
	// The gravity in controlled descent mode
	public float speedSmoothing= 10.0f;
	public float rotateSpeed= 500.0f;
	public float trotAfterSeconds= 3.0f;
	
	public bool canJump= true;
	
	private float jumpRepeatTime= 0.05f;
	private float jumpTimeout= 0.15f;
	private float groundedTimeout= 0.25f;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer= 0.0f;
	
	// The current move direction in x-z
	private Vector3 moveDirection= Vector3.zero;
	// The current vertical speed
	private float verticalSpeed= 0.0f;
	// The current x-z move speed
	private float moveSpeed= 0.0f;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping= false;
	private bool jumpingReachedApex= false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack= false;
	// Is the user pressing any keys?
	private bool isMoving= false;

	// Last time the jump button was clicked down
	private float lastJumpButtonTime= -10.0f;
	// Last time we performed a jump
	private float lastJumpTime= -1.0f;
	
	private Vector3 inAirVelocity= Vector3.zero;
	
	private float lastGroundedTime= 0.0f;
	
	private bool isControllable= true;
	
	//this variable stores the Animator on our character model
	internal Animator animController;
	
	void  Awake (){
		moveDirection = transform.TransformDirection(Vector3.forward);
		
		//find the character model's Animator component and store it
		animController = GetComponentInChildren<Animator>();
	}
	
	
	void  UpdateSmoothedMovementDirection (){
		Transform cameraTransform= Camera.main.transform;
		bool grounded= IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward= cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
	
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		
		//these are used to get player movement input every frame
		float verticalAxis = Input.GetAxisRaw("Vertical");
		float horizontalAxis = Input.GetAxisRaw("Horizontal");
	
		// Are we moving backwards or looking backwards
		if (verticalAxis < -0.2f)
			movingBack = true;
		else
			movingBack = false;
		
		bool wasMoving= isMoving;
		isMoving = Mathf.Abs (horizontalAxis) > 0.1f || Mathf.Abs (verticalAxis) > 0.1f;
			
		// Target direction relative to the camera
		Vector3 targetDirection= horizontalAxis * right + verticalAxis * forward;
		
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
	
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9f && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			float curSmooth= speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed= Mathf.Min(targetDirection.magnitude, 1.0f);
			
			//takes the vertical and horizontal axis movement input and determines the animation to play
			SetCharacterFreeMovementAnimation (verticalAxis, horizontalAxis, ref targetSpeed);
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		}
		// In air controls
		else
		{
			// Lock camera while in air
			if (jumping)
				lockCameraTimer = 0.0f;
	
			if (isMoving)
				inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
		}
	}
	
	//thie function determines the animation to play every frame based on the current character state
	void SetCharacterFreeMovementAnimation (float verticalAxis, float horizontalAxis, ref float targetSpeed)
	{
		//if the character is jumping, play jumping animation
		//in the current setup, this jumping state will never be reached because
		//the TPC script takes care of jumping by its own means
		if (_characterState == CharacterState.Jumping)
		{
			//no jumping state!!!
		}
		else if (verticalAxis != 0 || horizontalAxis != 0)	//if any input is detected on both axes (essentially, if the player is moving)...
		{
			//if a shift key is being pressed...
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				targetSpeed *= runSpeed;	//set speed to run speed
				_characterState = CharacterState.Running;	//set character state to Running
			}
			else	//else if there are no modifiers (base movement only)...
			{
				targetSpeed *= walkSpeed;	//set speed to walk speed
				_characterState = CharacterState.Walking;	//set character state to Walking
			}
		}
		else	//else if there is no input (no movement)...
		{
			_characterState = CharacterState.Idle;	//set character state to Idle
			targetSpeed = 0;
		}
		
		//this line sets the CharacterState parameter on the character model's animator controller every frame
		animController.SetInteger("CharacterState", (int)_characterState);
	}
	
	
	void  ApplyJumping (){
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
	
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing		
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	
	void  ApplyGravity (){
		if (isControllable)	// don't move player at all if not controllable.
		{		
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
		
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	
	float  CalculateJumpVerticalSpeed ( float targetJumpHeight  ){
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	void  DidJump (){
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpButtonTime = -10;
		
		_characterState = CharacterState.Jumping;
	}
	
	void  Update (){
		
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
	
		if (Input.GetButtonDown ("Jump"))
		{
			lastJumpButtonTime = Time.time;
		}
	
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
	
		// Apply jumping logic
		ApplyJumping ();
		
		// Calculate actual motion
		Vector3 movement= moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		movement *= Time.deltaTime;
		
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		collisionFlags = controller.Move(movement);
		
		// Set rotation to the move direction
		if (IsGrounded())
		{
			transform.rotation = Quaternion.LookRotation(moveDirection);
		}	
		else
		{
			Vector3 xzMove= movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001f)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}	
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				_characterState = CharacterState.Idle;	//reset the character state to Idle on landing (so it isn't stuck on Jumping)
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void  OnControllerColliderHit ( ControllerColliderHit hit   ){
	//	Debug.DrawRay(hit.point, hit.normal);
		if (hit.moveDirection.y > 0.01f) 
			return;
	}
	
	float  GetSpeed (){
		return moveSpeed;
	}
	
	internal bool  IsJumping (){
		return jumping;
	}
	
	bool  IsGrounded (){
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	Vector3  GetDirection (){
		return moveDirection;
	}
	
	internal bool  IsMovingBackwards (){
		return movingBack;
	}
	
	internal float  GetLockCameraTimer (){
		return lockCameraTimer;
	}
	
	bool IsMoving (){
		 return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
	
	bool  HasJumpReachedApex (){
		return jumpingReachedApex;
	}
	
	bool  IsGroundedWithTimeout (){
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	void  Reset (){
		gameObject.tag = "Player";
	}
}