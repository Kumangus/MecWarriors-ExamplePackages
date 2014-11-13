using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	public Animator charAnimator;
	public SceneManager sceneManager;

	void OnAnimatorMove ()
	{
		if (!rigidbody.isKinematic)
		{
			Vector3 velocity = charAnimator.deltaPosition / Time.deltaTime;
			velocity.y = rigidbody.velocity.y;
			velocity.z = 0;
			
			rigidbody.velocity = velocity;
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.name == "Action1")
		{
			sceneManager.SetSceneState(SceneManager.SceneState.TakeLightOrb);
			collider.enabled = false;
		}
		else if (collider.name == "Action2")
		{
			sceneManager.SetSceneState(SceneManager.SceneState.PlaceLightOrb);
			collider.enabled = false;
		}
		else if (collider.name == "Action3")
		{
			sceneManager.SetSceneState(SceneManager.SceneState.Char2Idle);
			collider.enabled = false;

			StartCoroutine(ShortPause());
		}
		else if (collider.tag == "EndScene")
		{
			sceneManager.SetSceneState(SceneManager.SceneState.EndScene);
			rigidbody.isKinematic = true;
		}
	}

	public void OrbAcquired ()
	{
		sceneManager.SetSceneState(SceneManager.SceneState.WalkToCage2);
	}

	IEnumerator ShortPause ()
	{
		yield return new WaitForSeconds(1.5f);
		sceneManager.SetSceneState(SceneManager.SceneState.TurnToRun);

		Transform char1 = GameObject.Find("UnityGuy1").transform;
		char1.eulerAngles = new Vector3(char1.eulerAngles.x, -char1.eulerAngles.y, char1.eulerAngles.z);
		sceneManager.SetSceneState(SceneManager.SceneState.Char2ChaseChar1);
	}
}
