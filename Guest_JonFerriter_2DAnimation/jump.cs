using System.Collections;

public class Jump : MonoBehaviour {

	public string jumpButton = "Fire1";	//stores the string name of our jump button for later use
	public float jumpPower = 10.0f;		//the power of a jump, affects jump height
	public Animator anim;				//our animator controller
	public float minJumpDelay = 0.5f;	//the minimum time between jumps
	public Transform groundCheck;		//stores the game object that checks if character is on the ground
	private float jumpTime = 0.0f;		//a timer for jump cooldown
	public bool grounded = false;		//checks if character is on the ground
	private bool jumped = false;		//checks if character has jumped (jump button pressed)
	private bool canJump = true;		//checks if character can jump now


	// Use this for initialization
	void Start () 
	{
		//stores the animator controller of our character
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//cast a line (as opposed to a ray) to see if the player is touching anything on the "ground" layer, returns TRUE or FALSE
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		jumpTime -= Time.deltaTime;		//continually subtract from the jump time

		//set the jump time to 0 if it is negative
		if(jumpTime < 0)
		{

			jumpTime = 0;

		}

		//if the jump button is pressed AND character can jump...
		if(Input.GetButtonDown(jumpButton) && canJump == true)
		{

			canJump = false;	//can't jump now
			jumped = true;		//has jumped
			grounded = false;	//is not on the ground
			anim.SetTrigger("Jump");	//fire the "Jump" trigger parameter in the animator controller to play jump animation
			rigidbody2D.AddForce(transform.up*jumpPower);	//add upward force to the character based on the jumpPower variable
			jumpTime = minJumpDelay;	//slight delay on the jump to make sure it goes through
		}

		//if character is on the ground AND the jump time is less than/equal to 0 AND character has jumped...
		if(grounded && jumpTime <=0 && jumped)
		{
			canJump = true;		//can now jump
			jumped = false;		//no longer in jump
			anim.SetTrigger("Land");	//fire the "Land" trigger parameter in the animator controller to play landing animation
		}
	}
	
}