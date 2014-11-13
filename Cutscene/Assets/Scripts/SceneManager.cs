using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	//the sequence of events this cutscene follows
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
	
	public SceneState currentSceneState;	//the current point in the sequence
	public Animator char1Animator;			//links to Char 1 Animator component
	public Animator doorAnimator;			//links to door Animator component
	public Animator char2Animator;			//links to Char 2 Animator component

	//triggers events in the sequence
	public void SetSceneState (SceneState state)
	{
		//sets the current scene state and then applies its value to all stored Animator components
		currentSceneState = state;
		char1Animator.SetInteger("SceneState", (int)currentSceneState);
		doorAnimator.SetInteger("SceneState", (int)currentSceneState);
		char2Animator.SetInteger("SceneState", (int)currentSceneState);
	}
}
