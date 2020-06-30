using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	void Start()
	{
	}

	public void Open()
	{
		gameObject.SetActive(true);

		Time.timeScale = 0f;
	}
	public void Close()
	{
		gameObject.SetActive(false);

		Time.timeScale = 1f;
	}

	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	
}