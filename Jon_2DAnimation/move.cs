using UnityEngine;
using System.Collections;

public class moveLeftRight : MonoBehaviour {

	public float speed = 1.0f;	//character movement speed
	public string axisName = "Horizontal";	//stores movement axis name so we don't have to type the string later
	public Animator anim;	//for our animator controller

	// Use this for initialization
	void Start () {
		//stores the animator controller of our character
		anim = gameObject.GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {

		//set the character's speed parameter in the animator controller based on horizontal axis input
		anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis(axisName)));

		//if horizontal axis input is negative (moving left)...
		if(Input.GetAxis(axisName) < 0) 
		{
			//set character's local x scale to -1 so it faces its reverse direction (left, not the default right)
			//otherwise the character will always walk to the right (its forward) even when moving left (its backward)
			Vector3 newScale = transform.localScale;
			newScale.x = -1.0f;
			transform.localScale = newScale;
		}
		//if horizontal axis input is positive (moving right)...
		else if (Input.GetAxis(axisName) > 0)
		{
			//set character's local x scale to 1 so it faces its forward direction (the default right)
			Vector3 newScale = transform.localScale;
			newScale.x = 1.0f;
			transform.localScale = newScale;
		}

		//set character position by adding the change in movement to the old position
		//(add the right axis * the axis input [gets left or right direction] * the movement speed * deltaTime [for smooth framerate])
		transform.position += transform.right*Input.GetAxis(axisName)*speed*Time.deltaTime;
	
	}
}