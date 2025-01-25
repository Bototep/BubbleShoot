using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
	[SerializeField] private float cutsceneDuration = 10f; 
	private float timeElapsed = 0f;

	void Update()
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= cutsceneDuration)
		{
			SceneManager.LoadScene(2);
		}
	}

	private IEnumerator LoadNextSceneAsync()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene3");
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
