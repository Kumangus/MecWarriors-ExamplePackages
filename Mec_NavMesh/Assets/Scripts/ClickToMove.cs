using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

	public CharNav charNav;

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;

			if (Physics.Raycast(ray, out rayHit, 100f))
			{
				charNav.navMeshAgent.destination = rayHit.point;
			}
		}
	}
}
