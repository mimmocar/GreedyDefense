using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private static GameObject thisObj;
	private static PauseMenu instance;
	private static GameControl gameControl;
	private bool isOn = true;

	public bool IsOn
	{
		get
		{
			return isOn;
		}
	}

	public static PauseMenu Instance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<PauseMenu>();
			thisObj = instance.gameObject;
			gameControl = GameControl.Instance();
		}

		return instance;
	}

	void Awake()
	{
		instance = this;
		thisObj = gameObject;
		gameControl = GameControl.Instance();

	}
	void Start()
	{
		thisObj = gameObject;
		OnOptionBackButton();
		Hide();
	}
	public void OnResumeButton()
	{
		//Hide();
		gameControl.ResumeGame();
	}
	public void OnMainMenuButton()
	{
		//Time.timeScale = 1;
		//GameControl.LoadMainMenu();
		gameControl.OpRequested = OperationRequested.Home;
	}
	public void OnRestartButton()
	{
		gameControl.gameState = _GameState.Restart;
		//GameControl.Load(SceneManager.GetActiveScene().name);
	}

	public void Hide()
	{
		isOn = false;
		thisObj.SetActive(isOn);
	}

	public void Show()
	{
		isOn = true;
		thisObj.SetActive(isOn);
	}
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
		thisObj.SetActive(true);
	}
}
