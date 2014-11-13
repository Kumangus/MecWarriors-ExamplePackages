using UnityEngine;
using System.Collections;

public class LightOrb : MonoBehaviour {
	
	public Transform leftHand;		//stores Char 1 left hand
	public Vector3 cage2Position;	//sets position of Orb in Cage 2

	//bools track Orb state
	public bool isOrbInHand = false;
	public bool isOrbInCage2 = false;

	//links to SceneManager object
	public SceneManager sceneManager;

	void Update ()
	{
		if (isOrbInHand)	//if the Orb is in Char 1's left hand, follow left hand position
		{
			transform.position = leftHand.position;
		}
		else if (isOrbInCage2)	//if the Orb is in Cage 2, set it's position to Cage 2
		{
			transform.position = cage2Position;
		}
	}

	//called by other scripts to set Orb's state and position throughout the scene
	public void SetOrbPosition ()
	{
		if (!isOrbInHand)	//sets the Orb to in hand the first time this function is called
		{
			isOrbInHand = true;
		}
		else	//if the Orb is already in hand, put it in Cage 2
		{
			isOrbInHand = false;
			isOrbInCage2 = true;

			//starts the door open animation
			sceneManager.SetSceneState(SceneManager.SceneState.OpenDoor);
		}
	}
}
