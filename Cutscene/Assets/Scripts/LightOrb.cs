using UnityEngine;
using System.Collections;

public class LightOrb : MonoBehaviour {

	public Transform leftHand;

	bool isOrbInHand = false;

	void Update ()
	{
		if (isOrbInHand)
		{
			transform.position = leftHand.position;
		}
	}

	public void SetOrbPosition ()
	{
		if (!isOrbInHand)
			isOrbInHand = true;
	}
}
