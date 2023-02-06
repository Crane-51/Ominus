using UnityEngine;
using UnityEngine.Events;
using DiContainerLibrary.DiContainer;
using Implementation.Data;

public class OnActivate : MonoBehaviour
{
	private bool activated = false;
	private bool waitingForBtnPress = false;
	private SpriteRenderer sr;
	[SerializeField] private Sprite inactiveSprite;
	[SerializeField] private Sprite activeSprite;
	[InjectDiContainter] private IPlayerKeybindsData keyBindings { get; set; }
	[InjectDiContainter] protected IGameInformation gameInformation { get; set; }
	[SerializeField] private bool activateOnTrigger = false;
	[SerializeField] private bool activateOnInteract = true;
	[SerializeField] private bool activateOnlyOnce = true;

	[Tooltip("Object can be switched between two states.\nOtherwise, player has to wait until it goes back to the default state.")]
	[SerializeField] private bool canBeSwitched = false;

	[SerializeField] private UnityEvent onInteraction = new UnityEvent();

	private void Start()
	{
		if (activateOnTrigger && activateOnInteract)
		{
			Debug.LogError("Object can be activated either by trigger or by interaction, not by both");
		}
		DiContainerInitializor.RegisterObject(this);
		keyBindings = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		sr = GetComponentInChildren<SpriteRenderer>();
		if (sr != null && inactiveSprite != null)
		{
			sr.sprite = inactiveSprite; 
		}
	}

	private void Update()
	{
		gameInformation.WaitingForInteraction = gameInformation.WaitingForInteraction || waitingForBtnPress;
		if (activateOnInteract && /*gameInformation.WaitingForInteraction &&*/ waitingForBtnPress && Input.GetKeyUp(keyBindings.KeyboardUse))
		{
			//put this in a new protected method that can be overriden
			activated = true;
			if (sr != null && activeSprite != null)
			{
				sr.sprite = activeSprite;
			}
			gameInformation.WaitingForInteraction = false;
			Interact();
			//if 2 way activation, make activated false again
			//if 1 way activation, wait for a signal to return to deactivated position || waitforseconds
		}
	}

	public void Interact()
	{
		//Debug.Log(onInteraction.GetPersistentEventCount());
		//Debug.Log(onInteraction.GetPersistentTarget(0));
		onInteraction.Invoke();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && !activated)
		{
			if (activateOnTrigger)
			{
				activated = true;
				if (sr != null && activeSprite != null)
				{
					sr.sprite = activeSprite;
				}
				Interact(); 
			}
			else if (activateOnInteract)
			{
				//gameInformation.WaitingForInteraction = true;
				waitingForBtnPress = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player" && activateOnInteract)
		{
			gameInformation.WaitingForInteraction = false;
			waitingForBtnPress = false;
		}
	}
}
