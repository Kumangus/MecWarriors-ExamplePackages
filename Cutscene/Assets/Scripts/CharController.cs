using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	public Animator charAnimator;
	public SceneManager sceneManager;

	float startZPos;

	void Start ()
	{
		startZPos = transform.position.z;
	}

	void OnAnimatorMove ()
	{
		if (!rigidbody.isKinematic)
		{
			Vector3 velocity = charAnimator.deltaPosition / Time.deltaTime;
			velocity.y = rigidbody.velocity.y;

			rigidbody.velocity = velocity;
		}
	}

	void LateUpdate ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, startZPos);
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.name == "Action1")
		{
			sceneManager.SetSceneState(SceneManager.SceneState.TakeLightOrb);
			collider.enabled = false;
		}
	}

	public void OrbAcquired ()
	{
		sceneManager.SetSceneState(SceneManager.SceneState.WalkToCage2);
	}
}
