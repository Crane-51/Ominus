using UnityEngine;
using General.State;
using DiContainerLibrary.DiContainer;
using Implementation.Data;

public class EnemySharedDataAndInit : StateForMovement
{
	[SerializeField] private GameObject healthBarPrefab;
	private EnemyHealthBar healthBar;
	[HideInInspector] public EnemyHealthBar HealthBar
	{
		get
			{
				if (healthBar == null)
				{
					GameObject bar = Instantiate(healthBarPrefab, GameObject.FindGameObjectWithTag("StatsCanvas").transform);
					healthBar = bar.GetComponent<EnemyHealthBar>();
				}

				return healthBar;				
			}
		private set
		{
			HealthBar = healthBar;
		}
	}

	/// <summary>
	/// Gets or sets enemy data.
	/// </summary>
	[InjectDiContainter]
	public IEnemyData enemyData { get; set; }

	/// <summary>
	/// Gets or sets enemy stats
	/// </summary>
	[InjectDiContainter]
	public IStats enemyStats { get; set; }

	/// <summary>
	/// Gets or sets weapon data
	/// </summary>
	public IWeaponData weaponData { get; set; }

	/// <summary>
	/// Force ranged enemies to take aim & shoot
	/// </summary>
	[HideInInspector] public bool forceAim = false;

	/// <summary>
	/// Player has been seen and locked as target
	/// </summary>
	[HideInInspector] public bool targetLocked = false;

	/// <summary>
	/// Last known player position while target is locked
	/// </summary>
	[HideInInspector] public Vector3 lastKnownPlayerPosition;

	/// <summary>
	/// Player is within vision range
	/// </summary>
	[HideInInspector] public bool targetInRangeOfVision = false;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		if (healthBar == null)
		{
			GameObject bar = Instantiate(healthBarPrefab, GameObject.FindGameObjectWithTag("StatsCanvas").transform);
			healthBar = bar.GetComponent<EnemyHealthBar>(); 
		}
		HealthBar.objectToFollow = transform;
		enemyData = SaveAndLoadData<IEnemyData>.LoadSpecificData(controller.Id);
		enemyStats = SaveAndLoadData<IStats>.LoadSpecificData(controller.Id);
		weaponData = SaveAndLoadData<IWeaponData>.LoadSpecificData(enemyStats.Weapon.ToString());
		lastKnownPlayerPosition = transform.position; // just to have a default value
	}
}
