using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Items;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMovement : HighPriorityState
{
    public static InventoryMovement singleton { get; set; }

    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    protected IPlayerKeybindsData keybinds { get; set; }

    private int InputValue { get; set; }

    private List<OldSlot> ItemsCurrentlyDisplaying { get; set; }

    private int CurrentItemIndex { get; set; }

    public float ChangeItemSpeed;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        ItemsCurrentlyDisplaying = GetComponentsInChildren<OldSlot>().ToList();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        CurrentItemIndex = 0;
        StartCoroutine(WaitForChildrenToInitialize());
        Priority = 0;
        singleton = this;
    }

    private void OnEnable()
    {
        if(controller != null)
        {
            controller.EndState(this);
        }
    }

    public override void Update_State()
    {
        InputValue = (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0) + (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0);

        if(controller.ActiveHighPriorityState != this && InputValue != 0)
        {
            controller.SwapState(this);
        }
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
        StartCoroutine(PerformASwapOfItems());
    }

    private IEnumerator WaitForChildrenToInitialize()
    {
        yield return new WaitForFixedUpdate();
        UpdateItems();
    }

    private IEnumerator PerformASwapOfItems()
    {
        UpdateItems();
        yield return new WaitForSeconds(ChangeItemSpeed);
        controller.EndState(this);
    }

    public void UpdateItems()
    {
        CurrentItemIndex += InputValue;
        foreach (var item in ItemsCurrentlyDisplaying)
        {
            item.DisplayItemInSlot(CurrentItemIndex);
        }
    }
}
