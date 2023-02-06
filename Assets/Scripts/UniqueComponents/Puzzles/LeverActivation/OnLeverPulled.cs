using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class OnLeverPulled : HighPriorityState
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

    /// <summary>
    /// Gets or sets component from <see cref="StateToActivate"/>
    /// </summary>
    private List<State> statesToActivate { get; set; }

    /// <summary>
    /// Gets or sets target that will activate on lever pulled.
    /// </summary>
    public List<GameObject> TargetedGameObject;

    /// <summary>
    /// Gets or sets focus target
    /// </summary>
    public List<MonoBehaviour> OnPullActivate;


    protected override void Initialization_State()
    {
        base.Initialization_State();
        keyBinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        statesToActivate = new List<State>();

        foreach (var state in OnPullActivate)
        {
            foreach (var target in TargetedGameObject)
            {
                var existingState = (State)target.GetComponent(state.name);
                if(existingState != null)
                {
                    statesToActivate.Add(existingState);
                }
            }
        }
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();

        foreach(var state in statesToActivate)
        {
            state.ActivateThisState(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject == gameInformation.Player && Input.GetKey(keyBinds.KeyboardUse) && controller.ActiveHighPriorityState != this)
        {
            controller.SwapState(this);
        }
    }
}
