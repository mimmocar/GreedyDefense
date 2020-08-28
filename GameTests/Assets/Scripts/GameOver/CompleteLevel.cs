using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		GameControl.Load(SceneManager.GetActiveScene().name);
	}

	public void Continue()
	{
		GameControl.ResumeGame();
		GameControl.Load("LevelSelector");
	}
}
