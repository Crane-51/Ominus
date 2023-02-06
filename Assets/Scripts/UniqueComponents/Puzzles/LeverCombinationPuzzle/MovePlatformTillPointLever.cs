using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Custom;
using Implementation.Data;
using UnityEngine;

public class MovePlatformTillPointLever : HighPriorityState
{
    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Gets or sets key binds.
    /// </summary>
    [InjectDiContainter]
    private IPlayerKeybindsData keyBinds { get; set; }

    private bool IsInRange { get; set; }

    private bool StateOfAnimation { get; set; }

    public IActivate PlatformMovement;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keyBinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
    }

    public override void OnEnter_State()
    {
        StateOfAnimation = !StateOfAnimation;

        if(StateOfAnimation)
        {
            base.OnEnter_State();
        }
        else
        {
            base.OnExit_State();
        }
        PlatformMovement.Activate();
        controller.EndState(this);
    }

    public override void Update_State()
    {
        if(IsInRange && Input.GetKeyDown(keyBinds.KeyboardUse))
        {
            controller.SwapState(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == gameInformation.Player)
        {
            IsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == gameInformation.Player)
        {
            IsInRange = false;
        }
    }

}
