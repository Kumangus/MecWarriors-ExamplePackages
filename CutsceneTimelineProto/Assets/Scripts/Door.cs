using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public SceneManager sceneManager;

	public void OpenComplete ()
	{
		sceneManager.SetSceneState(SceneManager.SceneState.EnterChar2);
	}
}
