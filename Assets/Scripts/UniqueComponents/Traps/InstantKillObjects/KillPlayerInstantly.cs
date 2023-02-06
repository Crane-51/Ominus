using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class KillPlayerInstantly : HighPriorityState
{
    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Assures only one object will kill player.
    /// </summary>
    public static bool alreadyHit;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        alreadyHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyHit)
			return;

        if (collision.gameObject == gameInformation.Player)
        {
            alreadyHit = true;
            if (gameInformation.PlayerStateController.ActiveHighPriorityState is CharacterIsDead)
				return;

            var takeDamage = gameInformation.Player.GetComponent<CharacterTakeDamage>();

            if (takeDamage != null)
            {
                //// Will definetly kill him.
                takeDamage.TakeDamage(10000);
                transform.localScale = new Vector3(transform.localScale.x * gameInformation.Player.transform.localScale.x, transform.localScale.y, 1);
                controller.SwapState(this);
            }
        }
    }
}
