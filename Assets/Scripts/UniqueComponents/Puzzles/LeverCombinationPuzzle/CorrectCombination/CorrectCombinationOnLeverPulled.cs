using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class CorrectCombinationOnLeverPulled : HighPriorityState
{
    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    protected IPlayerKeybindsData keybinds;

    /// <summary>
    /// Gets or sets state of animation.
    /// </summary>
    private bool StateOfAnimation { get; set; }

    /// <summary>
    /// Gets or sets all correct combinations
    /// </summary>
    public List<CorrectCombinationOnLeverPulledDisplayCurrentValue> correctCombinationOnLeverPulledDisplayCurrentValues;

    /// <summary>
    /// Gets or sets value defining is player in range to trigger this state.
    /// </summary>
    private bool IsInRange { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
    }

    public override void OnEnter_State()
    {
        StateOfAnimation = !StateOfAnimation;

        if(StateOfAnimation)
        {
            designController.StartTask(this);
        }
        else
        {
            designController.StopTask(this);
        }

        correctCombinationOnLeverPulledDisplayCurrentValues.ForEach(x => x.ChangeValue());

        controller.EndState(this);
    }

    public override void Update_State()
    {
        if(Input.GetKeyDown(keybinds.KeyboardUse) && IsInRange)
        {
            controller.SwapState(this);
        }
    }

    public bool IsInCorrectState()
    {
        if(correctCombinationOnLeverPulledDisplayCurrentValues.All(x=> x.CorrectValue == x.CurrentValue))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == gameInformation.Player.gameObject)
        {
            IsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == gameInformation.Player.gameObject)
        {
            IsInRange = false;
        }
    }
}
