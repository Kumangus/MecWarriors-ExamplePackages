using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	Animator animController;

	enum MovementState
	{
		Idle = 0,
		Walking = 1,
		Running = 2
	}
	MovementState currentMovementState;

	void Start ()
	{
		animController = GetComponent<Animator>();
	}

	void Update ()
	{
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");

		if (verticalAxis != 0 || horizontalAxis != 0)
		{
			if (Input.GetKey(KeyCode.LeftShift))
				currentMovementState = MovementState.Running;
			else
				currentMovementState = MovementState.Walking;
		}
		else
			currentMovementState = MovementState.Idle;

		animController.SetInteger("MovementState", (int)currentMovementState);
		animController.SetFloat("VerticalAxis", verticalAxis);
		animController.SetFloat("HorizontalAxis", horizontalAxis);

		Vector3 camForward = Camera.main.transform.forward;
		camForward.y = 0;
		transform.LookAt(transform.position + camForward);
	}

//	void OnAnimatorMove()
//	{
//		Vector3 velocity = animController.deltaPosition / Time.deltaTime;
//		velocity.y = rigidbody.velocity.y;
//
//		rigidbody.velocity = velocity;
//	}
}
