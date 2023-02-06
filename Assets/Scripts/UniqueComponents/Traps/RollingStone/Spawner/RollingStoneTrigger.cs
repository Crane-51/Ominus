using General.State;
using UnityEngine;

public class RollingStoneTrigger : RollingStoneSpawner
{
    protected override void Initialization_State()
    {
        DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
        designController = GetComponent<DesignController>();
        controller = GetComponent<StateController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller.SwapState(this);
    }
}
