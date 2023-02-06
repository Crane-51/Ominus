using System.Collections;
using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.UI.DialogBox
{
    public class DialogBoxManager : HighPriorityState
    {
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }

		[SerializeField] private Image CharacterImage;

		[SerializeField] private Text CharacterName;

		[SerializeField] private Text DialogText;
		[SerializeField] private GameObject bubblePrefab;
		//[SerializeField] private GameObject responseWindow;
		[SerializeField] private GameObject responseArea;
		[SerializeField] private GameObject responsePrefab;
		[SerializeField] private float responseOffset = 10f;

		private Dictionary<int, ISingleDialogData> currentDialog { get; set; }	
		private WholeDialogData wholeDialog;

        private int dialogIndex { get; set; }
		private bool isTyping;
		private bool cancelTyping;
		[SerializeField] private float typingDelay = 0.2f;
		private Dictionary<string, Transform> charTransforms;
		private GameObject bubble;
		private Image dialogBoxImage;
		private bool isBubbleDialogue = false;
		private List<GameObject> responsesList;

        public void StartDialog(string dialogId, int startLine = 0, int endLine = 0)
        {
			//currentDialog = SaveAndLoadData<IWholeDialogBoxData>.LoadSpecificData(dialogId).Dialog;
			wholeDialog = DialogImporter.instance.ImportDialog(dialogId, startLine, endLine);
			//currentDialog = DialogImporter.instance.ImportDialog(dialogId).Dialog;
			if (wholeDialog != null)
			{
				currentDialog = wholeDialog.Dialog;
				dialogIndex = 1;
				dialogBoxImage = GetComponent<Image>();
				dialogBoxImage.enabled = true;
				CharacterImage.enabled = true;
				isBubbleDialogue = false;
				this.gameObject.SetActive(true);
			}            
        }

		public void StartNextDialog(IWholeDialogBoxData dialogueData)
		{
			wholeDialog = (WholeDialogData) dialogueData;
			if (wholeDialog != null)
			{
				currentDialog = wholeDialog.Dialog;
				dialogIndex = 1;
				dialogBoxImage = GetComponent<Image>();
				dialogBoxImage.enabled = true;
				CharacterImage.enabled = true;
				isBubbleDialogue = false;
				//this.gameObject.SetActive(true);
				NextDialog();
			}
		}

		public void StartDialogBubble(string dialogId, Dictionary<string, Transform> transforms, int startLine = 0, int endLine = 0)
        {
			//currentDialog = SaveAndLoadData<IWholeDialogBoxData>.LoadSpecificData(dialogId).Dialog;
			wholeDialog = DialogImporter.instance.ImportDialog(dialogId, startLine, endLine);
			//currentDialog = DialogImporter.instance.ImportDialog(dialogId).Dialog;
			if (wholeDialog != null)
			{
				currentDialog = wholeDialog.Dialog;
				dialogIndex = 1;
				dialogBoxImage = GetComponent<Image>();
				dialogBoxImage.enabled = false;
				CharacterImage.enabled = false;
				DialogText.text = "";
				CharacterName.text = "";
				isBubbleDialogue = true;
				this.gameObject.SetActive(true);
			}
			charTransforms = transforms;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
			gameInformation.StopMovement = true;
			//Time.timeScale = 0;
			gameInformation.DialogActive = true;
			if (isBubbleDialogue)
			{
				NextDialogBubble(); 
			}
			else
			{
				NextDialog();
			}
        }

        public override void Update_State()
        {
            base.Update_State();
            if(controller.ActiveHighPriorityState != this)
            {
                controller.SwapState(this);
            }
			else if (Input.GetKeyDown(KeyCode.X) && !gameInformation.IsPaused)
			{
				if (isBubbleDialogue)
				{
					NextDialogBubble(); 
				}
				else
				{
					NextDialog();
				}
			} 
        }

        public override void WhileActive_State()
        {
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    NextDialog();
            //}
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            CharacterImage.sprite = null;
            CharacterName.text = string.Empty;
            DialogText.text = string.Empty;
			if (bubble != null)
			{
				bubble.SetActive(false); 
			}
            this.gameObject.SetActive(false);
			gameInformation.StopMovement = false;
			//Time.timeScale = 1;
			gameInformation.DialogActive = false;
		}

        private void NextDialog()
        {
			if (!isTyping)
			{
				if (dialogIndex > currentDialog.Count)
				{
					if (wholeDialog.Responses.Count == 0)
					{
						controller.EndState(this); 
					}
					else if (responsesList == null || responsesList.Count == 0)
					{
						DialogText.text = "";
						CharacterName.text = "Player";
						CharacterImage.sprite = Resources.Load<SpriteRenderer>("Player").sprite;
						responsesList = new List<GameObject>();
						for (int i = 0; i < wholeDialog.Responses.Count; i++)
						{
							int _i = i;
							//instantiate responses prefab
							GameObject go = Instantiate(responsePrefab, responseArea.transform);
							//GameObject go = Instantiate(responsePrefab, this.transform);
							go.transform.position = new Vector2(go.transform.position.x, go.transform.position.y - i * responseOffset);
							//set txt
							go.GetComponentInChildren<Text>().text = wholeDialog.Responses[i].Text;
							//set onclick
							go.GetComponent<Button>().onClick.AddListener(() => LoadNextWholeDialog(wholeDialog.Responses[_i].NextDialog));
							go.SetActive(true);
							responsesList.Add(go);
						}
						EventSystem.current.SetSelectedGameObject(responsesList[0]);
					}
				}
				else
				{
					CharacterImage.sprite = Resources.Load<SpriteRenderer>(currentDialog[dialogIndex].CharacterIcon).sprite;
					CharacterName.text = currentDialog[dialogIndex].CharacterName;
					StartCoroutine(TypeText(currentDialog[dialogIndex].Text));
					
					//DialogText.text = currentDialog[dialogIndex].Text;				
					dialogIndex++;					
				} 
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
			}
		}

		private void NextDialogBubble()
        {
			if (!isTyping)
			{
				if (dialogIndex > currentDialog.Count)
				{
					controller.EndState(this);
				}
				else
				{
					Vector3 offset = new Vector3(0f * charTransforms[currentDialog[dialogIndex].CharacterName].localScale.x, 2f);
					if (bubble == null)
					{
						bubble = Instantiate(bubblePrefab, Camera.main.WorldToScreenPoint(charTransforms[currentDialog[dialogIndex].CharacterName].position + offset), transform.rotation, this.GetComponentInParent<Transform>()); 
					}
					else
					{
						bubble.GetComponentInChildren<Text>().text = "";
						bubble.transform.position = Camera.main.WorldToScreenPoint(charTransforms[currentDialog[dialogIndex].CharacterName].position + offset);
					}
					bubble.GetComponent<DialogBubbleSize>().SwapX(charTransforms[currentDialog[dialogIndex].CharacterName].localScale.x > 0 ? 1 : -1);
					bubble.SetActive(true);
					//CharacterName.text = currentDialog[dialogIndex].CharacterName;
					StartCoroutine(TypeTextBubble(currentDialog[dialogIndex].Text));
					
					//DialogText.text = currentDialog[dialogIndex].Text;				
					dialogIndex++;					
				} 
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
			}
		}

		private IEnumerator TypeText(string text)
		{
			int letterIndex = 0;
			isTyping = true;
			cancelTyping = false;
			DialogText.text = "";

			while (isTyping && !cancelTyping && letterIndex < text.Length)
			{
				DialogText.text += text[letterIndex++];
				//yield return new WaitForSecondsRealtime(typingDelay);
				yield return new WaitForSeconds(typingDelay);
			}

			DialogText.text = text;
			isTyping = false;
			cancelTyping = false;
		}

		private IEnumerator TypeTextBubble(string text)
		{
			int letterIndex = 0;
			isTyping = true;
			cancelTyping = false;
			Text bubbleTxt = bubble.GetComponentInChildren<Text>();
			bubbleTxt.text = "";

			while (isTyping && !cancelTyping && letterIndex < text.Length)
			{
				bubbleTxt.text += text[letterIndex++];
				//yield return new WaitForSecondsRealtime(typingDelay);
				yield return new WaitForSeconds(typingDelay);
			}

			bubbleTxt.text = text;
			isTyping = false;
			cancelTyping = false;
		}

		private void DestroyResponses()
		{
			foreach (GameObject go in responsesList)
			{
				Destroy(go);
			}
			responsesList.Clear();
		}

		public void LoadNextWholeDialog(IWholeDialogBoxData dialogData)
		{
			DestroyResponses();

			if (dialogData == null)
			{
				controller.EndState(this);
			}
			else
			{
				StartNextDialog(dialogData);
			}
		}
    }
}
