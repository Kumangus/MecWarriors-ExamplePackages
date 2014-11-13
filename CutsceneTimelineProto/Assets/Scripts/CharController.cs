using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	public Animator charAnimator;		//stores Animator component
	public SceneManager sceneManager;	//links to SceneManager object

	void OnAnimatorMove ()
	{
		//only run if the rigidbody can accept velocity changes
		if (!rigidbody.isKinematic)
		{
			//TODO: re-comment this after finding issues...
			Vector3 velocity = (charAnimator.deltaPosition / Time.deltaTime);
			velocity.y = rigidbody.velocity.y;
			velocity.z = 0;

			//set the rigidbody's velocity based on calculations
			rigidbody.velocity = velocity;
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.name == "Action1")			//Action 1 - Char 1 stop and grab light orb
		{
			sceneManager.SetSceneState(SceneManager.SceneState.TakeLightOrb);
			collider.enabled = false;	//disable Action 1 trigger
		}
		else if (collider.name == "Action2")	//Action 2 - Char 1 stop and place light orb
		{
			sceneManager.SetSceneState(SceneManager.SceneState.PlaceLightOrb);
			collider.enabled = false;	//disable Action 2 trigger
		}
		else if (collider.name == "Action3")	//Action 3 - Char 2 stop and stare at Char 1
		{
			sceneManager.SetSceneState(SceneManager.SceneState.Char2Idle);
			collider.enabled = false;	//disable Action 3 trigger

			StartCoroutine(ShortPause());	//initiate pause for effect
		}
		else if (collider.tag == "EndScene")	//End Scene - stop all char movement
		{
			sceneManager.SetSceneState(SceneManager.SceneState.EndScene);
			rigidbody.isKinematic = true;
		}
	}

	//triggered by an anim event on the Grab High anim state
	public void OrbAcquired ()
	{
		//Char 1 walks to Cage 2
		sceneManager.SetSceneState(SceneManager.SceneState.WalkToCage2);
	}
	
	IEnumerator ShortPause ()
	{
		//pause for 1.5 seconds, then trigger the 'turn and run' sequence
		yield return new WaitForSeconds(1.5f);
		sceneManager.SetSceneState(SceneManager.SceneState.TurnToRun);

		//rotate Char 1 180 degrees around Y axis
		Transform char1 = GameObject.Find("UnityGuy1").transform;
		char1.eulerAngles = new Vector3(char1.eulerAngles.x, -char1.eulerAngles.y, char1.eulerAngles.z);

		//Char 2 chase Char 1
		sceneManager.SetSceneState(SceneManager.SceneState.Char2ChaseChar1);
	}
}
