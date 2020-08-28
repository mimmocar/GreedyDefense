using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private GameObject thisObj;
	private static PauseMenu instance;
	public GameObject pauseMenuObj;

	void Awake()
	{
		instance = this;
		thisObj = gameObject;

	}
	void Start()
	{
		OnOptionBackButton();
		Hide();
	}
	public void OnResumeButton()
	{
		Hide();
		GameControl.ResumeGame();
	}
	public void OnMainMenuButton()
	{
		Time.timeScale = 1;
		GameControl.LoadMainMenu();
	}
	public void OnRestartButton()
	{
		GameControl.Load(SceneManager.GetActiveScene().name);
	}

	public static void Hide() { instance._Hide(); }

	public static bool isOn = true;
	public static void Show() { instance._Show(); }
	public void _Show()
	{
		isOn = true;
		thisObj.SetActive(isOn);
	}
	public void _Hide()
	{
		isOn = false;
		thisObj.SetActive(isOn);
	}

	public void OnOptionBackButton()
	{
		pauseMenuObj.SetActive(true);
	}
}