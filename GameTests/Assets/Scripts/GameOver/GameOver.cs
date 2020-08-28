using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		GameControl.Load(SceneManager.GetActiveScene().name);
	}

	public void SelectWeapon()
	{
		GameControl.ResumeGame();
		GameControl.Load("WeaponSelection");
	}

}
