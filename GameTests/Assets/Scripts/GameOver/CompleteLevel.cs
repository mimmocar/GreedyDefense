using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
	public void SelectLevel()
	{
		GameControl.ResumeGame();
		GameControl.Load("LevelSelector");
	}
	public void Retry()
	{
		GameControl.ResumeGame();
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}

	public void Continue()
	{
		GameControl.ResumeGame();
		GameControl.Load("LevelSelector");
	}
}
