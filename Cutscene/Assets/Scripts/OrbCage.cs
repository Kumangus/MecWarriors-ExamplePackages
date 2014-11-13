//This script is used on BOTH Cages since they use the same checks.
//The difference between the two uses is the effect that calling SetOrbPosition() will have.

using UnityEngine;
using System.Collections;

public class OrbCage : MonoBehaviour {

	//links to Light Orb object in scene
	public LightOrb lightOrb;

	void OnTriggerEnter (Collider collider)
	{
		//if Char 1 left hand collides with Cage, disable Cage collider and set orb position
		if (collider.name == "LeftHand")
		{
			this.collider.enabled = false;
			lightOrb.SetOrbPosition();
		}
	}
}
