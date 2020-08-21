using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
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

	public void SelectWeapon()
	{
		GameControl.ResumeGame();
		GameControl.Load("WeaponSelection");
	}

}
