using UnityEngine;
using System.Collections;

public class TSpriteAnim : MonoBehaviour {

	public Material animMaterial;

	public int animFPS = 24;
	int originalFPS;
	int lastFPS;

	float frameDuration;

	public enum AnimType {Once, Loop, PingPongOnce, PingPongLoop}
	public AnimType animType;

	public int tex_NumRows = 1;
	public int tex_NumColumns = 1;
	int totalFrames;

	public enum StartPoint {TopLeft, BottomLeft}
	public StartPoint startPoint;

	float frameWidthOffset;
	float frameHeightOffset;

	enum AnimState {Waiting, Animating, Paused, Rewinding}
	AnimState animState;
	AnimState animStateOnPause;

	int currentAnimFrame;
	float lastFrameTime;

	void Start ()
	{
		originalFPS = animFPS;
		CalculateNewFrameDuration(animFPS);
//		Debug.Log(frameTime);

		totalFrames = tex_NumRows * tex_NumColumns;

		frameWidthOffset = 1f/tex_NumColumns;
		frameHeightOffset = 1f/tex_NumRows;
//		Debug.Log(frameWidthOffset + " : " + frameHeightOffset);

		currentAnimFrame = 0;
	}

	void Update ()
	{
		if (lastFPS != animFPS)
			CalculateNewFrameDuration(animFPS);

		switch (animState)
		{
			case AnimState.Animating:
				AnimateForward();
				break;

			case AnimState.Rewinding:
				AnimateReverse();
				break;

			case AnimState.Paused:
			case AnimState.Waiting:
			default:
				break;
		}

		lastFPS = animFPS;
	}

	//TODO:Remove test GUI
	void OnGUI ()
	{
		if (GUILayout.Button("Play Anim"))
			PlayAnimation();

		if (GUILayout.Button("Rewind Anim"))
			RewindAnimation();

		if (GUILayout.Button("Pause/Unpause Anim"))
		{
			if (animState != AnimState.Paused)
				PauseAnimation();
			else
				UnpauseAnimation();
		}
	}

	void Animate (int posNegOne)
	{
		int yOffsetMultiplier = 0;

		if (startPoint == StartPoint.TopLeft)
			yOffsetMultiplier = tex_NumRows - 1 -(currentAnimFrame/tex_NumColumns);
		else if (startPoint == StartPoint.BottomLeft)
			yOffsetMultiplier = currentAnimFrame/tex_NumColumns;

		if (Time.time - lastFrameTime >= frameDuration)
		{
			currentAnimFrame += posNegOne;
			lastFrameTime = Time.time;
		}

		#if UNITY_EDITOR
		if (!animMaterial)
			Debug.LogError("There is no material to animate!");
		else
		#endif
			animMaterial.mainTextureOffset = new Vector2(frameWidthOffset * currentAnimFrame,
			                                             frameHeightOffset * yOffsetMultiplier);
	}

	void AnimateForward ()
	{
		Debug.Log("Playing forward animation");

		Animate(1);

		//TODO: extract into its own function
		if (currentAnimFrame == totalFrames - 1)
		{
			switch (animType)
			{
				case AnimType.Once:
					animState = AnimState.Waiting;
					break;
					
				case AnimType.Loop:
					currentAnimFrame = -1;
					break;
					
				case AnimType.PingPongOnce:
				case AnimType.PingPongLoop:
					RewindAnimation(animType);
					break;
				}
		}
	}

	void AnimateReverse ()
	{
		Debug.Log("Playing backward animation");
		
		Animate(-1);

		//TODO: extract into its own function
		if (currentAnimFrame == 0)
		{
			switch (animType)
			{
			case AnimType.Once:
				animState = AnimState.Waiting;
				break;
				
			case AnimType.Loop:
				currentAnimFrame = totalFrames;
				break;
				
			case AnimType.PingPongOnce:
				animState = AnimState.Waiting;
				break;
				
			case AnimType.PingPongLoop:
				PlayAnimation();
				break;
			}
		}
	}

	void CalculateNewFrameDuration (int fps)
	{
		animFPS = fps;
		lastFPS = animFPS;
		frameDuration = 1f/animFPS;
	}

	public void PlayAnimation ()
	{
		PlayAnimation(originalFPS, animType);
	}

	public void PlayAnimation (AnimType setAnimType)
	{
		PlayAnimation(originalFPS, setAnimType);
	}

	public void PlayAnimation (int fps)
	{
		PlayAnimation(fps, animType);
	}

	public void PlayAnimation (int fps, AnimType setAnimType)
	{
		//TODO: make sure that fps can't go below 1 in custom inspector!
//		if (fps < 1)
//			Debug.LogError("FPS can't be less than 1! Animation will not play.");
//		else
//		{
			animState = AnimState.Animating;
			animType = setAnimType;

			if (currentAnimFrame == totalFrames - 1)
			Debug.LogError("Animation is at the last frame! Use ResetAnimation() and then start from the beginning.");
//			currentAnimFrame = 0;
			
			CalculateNewFrameDuration(fps);
			lastFrameTime = Time.time;
//		}
	}

	public void PauseAnimation ()
	{
		animStateOnPause = animState;
		animState = AnimState.Paused;
		Debug.Log("Paused : " + animStateOnPause);
	}

	public void UnpauseAnimation ()
	{
		animState = animStateOnPause;
		animStateOnPause = AnimState.Waiting;
		Debug.Log("Unpaused");
	}


	public void RewindAnimation ()
	{
		RewindAnimation(originalFPS, AnimType.Once);
	}

	public void RewindAnimation (AnimType setAnimType)
	{
		RewindAnimation(originalFPS, setAnimType);
	}

	public void RewindAnimation (int fps)
	{
		RewindAnimation(fps, AnimType.Once);
	}

	public void RewindAnimation (int fps, AnimType setAnimType)
	{
		//TODO: make sure that fps can't go below 1 in custom inspector!
//		if (fps < 1)
//			Debug.LogError("FPS can't be less than 1! Animation will not play.");
		if (currentAnimFrame == 0)
			Debug.LogError("Animation can't rewind from the beginning!");
		else
		{
			animState = AnimState.Rewinding;
			animType = setAnimType;

			CalculateNewFrameDuration(fps);
			lastFrameTime = Time.time;
		}
	}

//	public void ResetAnimation ()
//	{}
//
//	public int GetAnimationFrame ()
//	{
//		return currentAnimFrame;
//	}
//
//	public void SetAnimationFrame (int frame)
//	{
//		currentAnimFrame = frame;
//	}
}
