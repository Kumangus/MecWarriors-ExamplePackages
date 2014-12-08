using UnityEngine;
using System.Collections;

public class CharNav : MonoBehaviour {

	public Animator animController;

	public enum MoveState {Idle, Walking}
	public MoveState moveState;

	public NavMeshAgent navMeshAgent;

	public bool moveOnStart;
	public Vector3 startDestination;

	void Start ()
	{
		if (moveOnStart)
			navMeshAgent.destination = startDestination;
	}

	void Update ()
	{
		if (navMeshAgent.hasPath)
			moveState = MoveState.Walking;
		else
			moveState = MoveState.Idle;

		animController.SetInteger("MoveState", (int)moveState);
	}

	void OnAnimatorMove ()
	{
		if (moveState == MoveState.Walking)
		{
			navMeshAgent.velocity = animController.deltaPosition / Time.deltaTime;
			transform.rotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
		}
	}
}
