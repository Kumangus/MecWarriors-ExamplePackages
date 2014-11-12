using UnityEngine;
using System.Collections;

public class LightOrb : MonoBehaviour {

	public Transform leftHand;
	public Vector3 cage2Position;

	public bool isInHand = false;
	public bool isInCage2 = false;

	public SceneManager sceneManager;

	void Update ()
	{
		if (isInHand)
		{
			transform.position = leftHand.position;
		}
		else if (isInCage2)
		{
			transform.position = cage2Position;
		}
	}

	public void SetOrbPosition ()
	{
		if (!isInHand)
		{
			isInHand = true;
		}
		else
		{
			isInHand = false;
			isInCage2 = true;
			sceneManager.SetSceneState(SceneManager.SceneState.OpenDoor);
		}
	}
}
