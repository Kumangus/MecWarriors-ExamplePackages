using UnityEngine;
using System.Collections;

public class CharNav : MonoBehaviour {

	//link to Animator component
	public Animator animController;

	//used to set anim controller parameters
	public enum MoveState {Idle, Walking}
	public MoveState moveState;

	//link to NavMeshAgent component
	public NavMeshAgent navAgent;

	//set TRUE to auto-move character to a point on scene start
	public bool moveOnStart;
	public Vector3 startDestination;

	void Start ()
	{
		//move to start destination automatically
		if (moveOnStart)
			navAgent.destination = startDestination;
	}

	void Update ()
	{
		//set animation speed based on navAgent 'Speed' var
		animController.speed = navAgent.speed;

		//character walks if there is a navigation path set, idle all other times
		if (navAgent.hasPath)
			moveState = MoveState.Walking;
		else
			moveState = MoveState.Idle;

		//send move state info to animator controller
		animController.SetInteger("MoveState", (int)moveState);
	}

	void OnAnimatorMove ()
	{
		//only perform if walking
		if (moveState == MoveState.Walking)
		{
			//set the navAgent's velocity to the velocity of the animation clip currently playing
			navAgent.velocity = animController.deltaPosition / Time.deltaTime;

			//smoothly rotate the character in the desired direction of motion
			Quaternion lookRotation = Quaternion.LookRotation(navAgent.desiredVelocity);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime);
		}
	}
}
