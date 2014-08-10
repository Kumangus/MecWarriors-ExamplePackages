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

		if (verticalAxis != 0)
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
	}

//	void OnAnimatorMove()
//	{
//		Vector3 velocity = animController.deltaPosition / Time.deltaTime;
//		velocity.y = rigidbody.velocity.y;
//
//		rigidbody.velocity = velocity;
//	}
}
