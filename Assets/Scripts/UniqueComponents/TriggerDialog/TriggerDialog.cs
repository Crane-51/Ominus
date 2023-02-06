using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class TriggerDialog : HighPriorityState {

    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject == gameInformation.Player)
        {
            UIController.singleton.StartDialog(controller.Id);
            Destroy(this.gameObject);
        }
    }
}
