using UnityEngine;
using System.Collections;

public class Final_PlayerMovement : MonoBehaviour {
	
	Transform mainCamera;		//holds the main camera as a Transform object
	Animator animController;	//holds the player character's animator controller

	//the character's possible movement states
	enum MovementState
	{
		Idle = 0,
		Walking = 1,
		Running = 2
	}

	//holds the player's current enumerated movement state
	MovementState currentMovementState;
	
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.transform;		//initialize the camera's transform
		animController = GetComponent<Animator>();	//initialize the animator controller
	}
	
	// Update is called once per frame
	void Update ()
	{
		//get axis input from player (W, A, S, and D keys/Arrow keys)
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");

		//get the forward-facing direction of the camera
		Vector3 cameraForward = mainCamera.TransformDirection(Vector3.forward);
		cameraForward.y = 0;	//set to 0 because of camera rotation on the X axis

		//get the right-facing direction of the camera
		Vector3 cameraRight = mainCamera.TransformDirection(Vector3.right);

		//determine the direction the player will face based on input and the camera's right and forward directions
		Vector3 targetDirection = horizontalAxis * cameraRight + verticalAxis * cameraForward;

		//normalize the direction the player should face
		Vector3 lookDirection = targetDirection.normalized;

		//rotate the player to face the correct direction ONLY if there is any player input
		if (lookDirection != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(lookDirection);

		//if there is any player input...
		if (verticalAxis != 0 || horizontalAxis != 0)
		{
			//if the run modifier key is pressed
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				currentMovementState = MovementState.Running;	//set movement state to running
			else	//if there is no movement modifier
				currentMovementState = MovementState.Walking;	//set movement state to walking
		}
		else	//default to idle if no input
		{
			currentMovementState = MovementState.Idle;	//set movement state to idle
		}

		//send the current movement state to the character controller
		animController.SetInteger("MovementState", (int)currentMovementState);
	}
}