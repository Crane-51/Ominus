using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class Teleporter : HighPriorityState
{
    /// <summary>
    /// Gets or sets target.
    /// </summary>
    [SerializeField] private Teleporter Target;

    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Gets or sets key binds.
    /// </summary>
    [InjectDiContainter]
    private IPlayerKeybindsData keybinds { get; set; }

	private bool waitingKeyPressed = false;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
    }

	public override void Update_State()
	{
		base.Update_State();
		gameInformation.WaitingForInteraction = gameInformation.WaitingForInteraction || waitingKeyPressed;
		if (controller.ActiveHighPriorityState != this && waitingKeyPressed && (Input.GetKeyDown(keybinds.KeyboardUse) || Input.GetKeyDown(keybinds.JoystickUse)))
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
    {
        base.OnEnter_State();
		gameInformation.WaitingForInteraction = false;
		StartCoroutine(AnimationColldown());
        Priority = 10;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
		if (!waitingKeyPressed && collision.gameObject == gameInformation.Player && Target != null)
		{
			waitingKeyPressed = true;
			//gameInformation.WaitingForInteraction = true;
		}


		//if (collision.gameObject == gameInformation.Player && Target != null  && (Input.GetKeyDown(keybinds.KeyboardUse) || Input.GetKeyDown(keybinds.JoystickUse)))
  //      {
  //          controller.SwapState(this);
  //      }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == gameInformation.Player && Target != null)
		{
			waitingKeyPressed = true;
			//gameInformation.WaitingForInteraction = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject == gameInformation.Player)
		{
			waitingKeyPressed = false;
			gameInformation.WaitingForInteraction = false;
		}
	}

	public override void OnExit_State()
    {
        base.OnExit_State();
        if (Target != null)
        {
            Target.ExitTeleporter();
        }
    }

    private IEnumerator AnimationColldown()
    {
        //yield return new WaitUntil(AnimationCheck);
        yield return new WaitForSeconds(1);
        controller.EndState(this);
    }

    private bool AnimationCheck()
    {
        return designController.animationController.IsAnimationOver(this);
    }

    public void ExitTeleporter()
    {
        gameInformation.Player.transform.position = transform.position;
    }
}
