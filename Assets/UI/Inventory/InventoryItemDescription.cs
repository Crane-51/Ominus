using General.State;
using UnityEngine.UI;

public class InventoryItemDescription : HighPriorityState
{
    private Text text { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        text = GetComponentInChildren<Text>();
    }

    public void SetDescription(string description)
    {
        text.text = description;
    }
}
