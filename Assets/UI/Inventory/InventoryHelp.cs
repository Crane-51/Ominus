using System.Collections;
using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHelp : HighPriorityState
{
    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    protected IPlayerKeybindsData keybinds { get; set; }

    private Text text { get; set; }

    private const string HelpInfo = "Use Item - {0} \t \t Destroy item - {1}";

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        text = GetComponent<Text>();
        text.text = string.Format(HelpInfo, keybinds.KeyboardUse, keybinds.KeyboardPunchKey);
    }


}
