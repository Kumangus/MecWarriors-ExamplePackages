using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	//links to SceneManager object
	public SceneManager sceneManager;
	bool hasPressedButton = false;
	
	void OnGUI ()
	{
		//if start button has not been pressed display GUI button
		if (!hasPressedButton)
		{
			if (GUI.Button(new Rect(0, Screen.height - Screen.height/8, Screen.width, Screen.height/8), "Start Cutscene"))
			{
				//begins the sequence of events in the cutscene
				sceneManager.SetSceneState(SceneManager.SceneState.WalkToCage1);
				hasPressedButton = true;
			}
		}
	}
}
