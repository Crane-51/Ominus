using Assets.UI.DialogBox;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using Menus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

    /// <summary>
    /// Defines singleton for usage of ui elements.
    /// </summary>
    public static UIController singleton { get; set; }

    /// <summary>
    /// Gets or sets inventory stats.
    /// </summary>
    public GameObject Inventory;

    /// <summary>
    /// Gets or sets  pause menu.
    /// </summary>
    public GameObject PauseMenu;

    /// <summary>
    /// Gets or sets dialog box manager.
    /// </summary>
    public DialogBoxManager dialogBoxManager;

    /// <summary>
    /// Gets or sets player key binds.
    /// </summary>
    [InjectDiContainter]
    private IPlayerKeybindsData keyBinings { get; set; }

	/// <summary>
	/// Gets or sets game information data.
	/// </summary>
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	public void Awake()
    {
        singleton = this;
        keyBinings = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
    }

	private void Start()
	{
		DiContainerInitializor.RegisterObject(this);
	}

	private void Update()
    {
		//TODO: uncomment when inventory is added
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    Inventory.SetActive(!Inventory.activeSelf);
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
			gameInformation.StopMovement = PauseMenu.activeSelf || dialogBoxManager.gameObject.activeSelf;
			Time.timeScale = PauseMenu.activeSelf /*|| dialogBoxManager.gameObject.activeSelf */? 0 : 1;
			gameInformation.IsPaused = PauseMenu.activeSelf;
			if (PauseMenu.activeSelf)
			{
				EventSystem.current.SetSelectedGameObject(PauseMenu.GetComponentInChildren<Button>().gameObject); 
			}

		}
    }

    public void StartDialog(string dialogId, int startLine = 0, int endLine = 0)
    {
		//dialogBoxManager.gameObject.SetActive(true);
		dialogBoxManager.StartDialog(dialogId, startLine, endLine); 		
    }

	public void StartDialogBubble(string dialogId, Dictionary<string, Transform> charTransforms, int startLine = 0, int endLine = 0)
    {
		//dialogBoxManager.gameObject.SetActive(true);
		dialogBoxManager.StartDialogBubble(dialogId, charTransforms, startLine, endLine); 		
    }
}
