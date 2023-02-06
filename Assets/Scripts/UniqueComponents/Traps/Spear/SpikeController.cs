using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class SpikeController : HighPriorityState
{
    /// <summary>
    /// Defines if multiple objects will be spawn
    /// </summary>
    [SerializeField] private bool spawnMultiple;

	/// <summary>
	/// Defines damage of spear.
	/// </summary>
	[SerializeField] private int damage;

    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

	/// <summary>
	/// Gets or sets spawn points
	/// </summary>
	private SpearSpawner[] spawnPoints;

	/// <summary>
	/// Gets or sets trigger point.
	/// </summary>
	private Collider2D triggerPoint;

	/// <summary>
	/// Gets or sets collider 2d.
	/// </summary>
	private Collider2D targetCollider;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        targetCollider = gameInformation.Player.GetComponent<Collider2D>();
        spawnPoints = GetComponentsInChildren<SpearSpawner>();
        triggerPoint = GetComponentInChildren<Collider2D>();
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();

        foreach (var spawn in spawnPoints)
        {
            spawn.StartState(damage, spawnMultiple);
        }
    }

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		if (!triggerPoint.IsTouching(targetCollider))
		{
			foreach (var spawn in spawnPoints)
			{
				spawn.StopState();
			}
			controller.EndState(this);
		}
	}

	public override void Update_State()
    {
        if (controller.ActiveHighPriorityState != this && triggerPoint.IsTouching(targetCollider))
        {
            controller.SwapState(this);
        }
    }
}
