using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	public SceneManager sceneManager;
	bool hasPressedButton = false;
	
	void OnGUI ()
	{
		if (!hasPressedButton)
		{
			if (GUI.Button(new Rect(0, Screen.height - Screen.height/8, Screen.width, Screen.height/8), "Start Cutscene"))
			{
				sceneManager.SetSceneState(SceneManager.SceneState.WalkToCage1);
				hasPressedButton = true;
			}
		}
	}
}
