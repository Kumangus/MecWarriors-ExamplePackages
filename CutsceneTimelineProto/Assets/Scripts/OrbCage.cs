using UnityEngine;
using System.Collections;

public class OrbCage : MonoBehaviour {

	public LightOrb lightOrb;

	void OnTriggerEnter (Collider collider)
	{
		if (collider.name == "LeftHand")
		{
			this.collider.enabled = false;
			lightOrb.SetOrbPosition();
		}
	}
}
