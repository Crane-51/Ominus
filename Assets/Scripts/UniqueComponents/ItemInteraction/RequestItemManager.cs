using System.Linq;
using DiContainerLibrary.DiContainer;
using Implementation.Custom;
using Implementation.Data;
using UnityEngine;

public class RequestItemManager : Activate
{
    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    protected IPlayerKeybindsData keybinds { get; set; }

    public string ItemName;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
        var requiredItem = gameInformation.InventoryData.Slots.FirstOrDefault(x => x.ItemsResource == ItemName);

        if (requiredItem != null)
        {
            gameInformation.InventoryData.Slots.Remove(requiredItem);
            statesToActivate.ForEach(x => x.Activate());
        }
        controller.EndState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == gameInformation.Player)
        {
            controller.SwapState(this);
        }
    }
}
