using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public enum SceneState
	{
		IdleStart = 0,
		WalkToCage1 = 1,
		TakeLightOrb = 2,
		WalkToCage2 = 3,
		PlaceLightOrb = 4,
		OpenDoor = 5,
		EnterChar2 = 6,
		Char2Idle = 7,
		TurnToRun = 8,
		Char2ChaseChar1 = 9,
		EndScene = 10
	}

	public SceneState currentSceneState;
	public Animator char1Animator;
	public Animator char2Animator;
	public Animator doorAnimator;

	public void SetSceneState (SceneState state)
	{
		currentSceneState = state;
		char1Animator.SetInteger("SceneState", (int)currentSceneState);
		char2Animator.SetInteger("SceneState", (int)currentSceneState);
		doorAnimator.SetInteger("SceneState", (int)currentSceneState);
	}
}
