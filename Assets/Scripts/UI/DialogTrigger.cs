using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using System.Collections.Generic;

public class DialogTrigger : MonoBehaviour
{
	[SerializeField] private string dialogId;
	[SerializeField] [Tooltip("Line numbering starts with 1")] private int startLine;
	[SerializeField] [Tooltip("Inclusive")] private int endLine;
	[SerializeField] private bool onlyOnce = true;
	[SerializeField] private bool requireBtnPress = false;
	[SerializeField] private List<string> characterNames;
	[SerializeField] private List<Transform> transforms;
	private Dictionary<string, Transform> charTransforms = new Dictionary<string, Transform>();
	private UIController uiCtrl;
	private bool waitingForBtnPress = false;
	[SerializeField] private bool bubbleDialogue = false;
	/// <summary>
	/// Gets or sets player key binds.
	/// </summary>
	[InjectDiContainter]
	private IPlayerKeybindsData keyBindings { get; set; }

	/// <summary>
	/// Gets or sets game information data.
	/// </summary>
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }
	
	private void Start()
	{
		DiContainerInitializor.RegisterObject(this);
		uiCtrl = FindObjectOfType<UIController>();
		keyBindings = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		if (characterNames.Count == transforms.Count && characterNames.Count != 0)
		{
			for (int i = 0; i < characterNames.Count; i++)
			{
				charTransforms.Add(characterNames[i], transforms[i]);
			}
		}
	}

	private void Update()
	{
		gameInformation.WaitingForInteraction = gameInformation.WaitingForInteraction || waitingForBtnPress;
		if (/*gameInformation.WaitingForInteraction &&*/ waitingForBtnPress && !gameInformation.IsPaused && !gameInformation.DialogActive && Input.GetKeyUp(keyBindings.KeyboardUse))
		{
			if (dialogId != "" && uiCtrl != null)
			{
				//uiCtrl.StartDialog(dialogId, startLine, endLine);				
				if (bubbleDialogue)
				{
					uiCtrl.StartDialogBubble(dialogId, charTransforms, startLine, endLine); 
				}
				else
				{
					uiCtrl.StartDialog(dialogId, startLine, endLine);
				}
			}

			if (onlyOnce)
			{
				gameInformation.WaitingForInteraction = false;
				Destroy(this.gameObject);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (requireBtnPress)
			{
				//gameInformation.WaitingForInteraction = true;
				waitingForBtnPress = true;
				return;
			}

			if (dialogId != "" && uiCtrl != null)
			{
				if (!bubbleDialogue)
				{
					uiCtrl.StartDialog(dialogId, startLine, endLine); 
				}
				else
				{
					uiCtrl.StartDialogBubble(dialogId, charTransforms, startLine, endLine);
				}
			}

			if (onlyOnce)
			{
				Destroy(this.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			gameInformation.WaitingForInteraction = false;
			waitingForBtnPress = false;
		}
	}
}
