using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class InventoryOnItemSelected : HighPriorityState
{
    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    protected IPlayerKeybindsData keybinds { get; set; }

    /// <summary>
    /// Gets or sets all active slots in inventory.
    /// </summary>
    private OldSlot SlotInFocus { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        SlotInFocus = GetComponentsInChildren<OldSlot>().FirstOrDefault(x=> x.position == General.Enums.InventorySlotPosition.Focus);
        Priority = 5;
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
        SlotInFocus.Use();
        InventoryMovement.singleton.UpdateItems();
        controller.EndState(this);
    }

    public override void Update_State()
    {
        if(this.gameObject.activeSelf && Input.GetKeyDown(keybinds.KeyboardUse))
        {
            controller.SwapState(this);
        }
    }
}
