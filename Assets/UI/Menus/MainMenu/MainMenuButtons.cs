using System;
using System.Collections;
using Implementation.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DiContainerLibrary.DiContainer;

public class MainMenuButtons : MonoBehaviour
{

	public string Start_Click_Scene = "TashaTesting";
	public string Main_Menu_Click_Scene = "MainMenu";

	public InputField inputField;

	public SwapEffects effects;

	/// <summary>
	/// Gets or sets game information data.
	/// </summary>
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	private void Start()
	{
		effects = GameObject.FindObjectOfType<SwapEffects>();
		DiContainerInitializor.RegisterObject(this);
	}

	public void OnContinueClick()
	{
		transform.parent.gameObject.SetActive(false);
		gameInformation.IsPaused = false;
		gameInformation.StopMovement = gameInformation.IsPaused || gameInformation.DialogActive;
		//Time.timeScale = gameInformation.IsPaused || gameInformation.DialogActive ? 0 : 1;		
		Time.timeScale = 1;
	}

	public void OnStartClick()
	{
		Time.timeScale = 1;
		effects.OnActivation(LoadNewScene, General.Enums.EffectDirection.FadeOut);
		//Time.timeScale = 1;
	}

	private IEnumerator WaitBeforeActivating()
	{
		yield return new WaitForSeconds(2);
		GetComponent<Button>().interactable = true;
	}

	public void OnMainMenuClick()
	{
		SceneManager.LoadScene(Main_Menu_Click_Scene);
		Time.timeScale = 1;
	}

	public void OnExitClick()
	{
		Application.Quit();
	}

	private void LoadNewScene()
	{
		SceneManager.LoadScene(Start_Click_Scene);
	}
}
