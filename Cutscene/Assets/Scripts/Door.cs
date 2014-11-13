using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	//links to SceneManager object
	public SceneManager sceneManager;

	//triggered by an anim event on the Open Door anim
	public void OpenComplete ()
	{
		//begin Char 2 entry
		sceneManager.SetSceneState(SceneManager.SceneState.EnterChar2);
	}
}
