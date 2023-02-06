using System.Collections;
using General.Enums;
using General.State;
using UnityEngine;
using UnityEngine.Assertions;

public class SpearSpawner : HighPriorityState
{
	/// <summary>
	/// Defines object to spawn (Must have spear script on it).
	/// </summary>
	//public Spear ObjectToSpawn;

	[SerializeField] private PoolObjectKey objectToSpawn;

	/// <summary>
	/// Defines damage of the spears.
	/// </summary>
	[SerializeField] private int damage;

	/// <summary>
	/// Defines movement speed of spawned objects.
	/// </summary>
	[SerializeField] private float movementSpeed;

	/// <summary>
	/// Sets spawn offset time.
	/// </summary>
	[SerializeField] private float spawnOffsetTime;

	/// <summary>
	/// Gets or sets first spawn offset.
	/// </summary>
	[SerializeField] private float firstSpawnOffset;

	/// <summary>
	/// Defines direction of movement.
	/// </summary>
	[SerializeField] private DirectionEnum horizontalDirection;

	/// <summary>
	/// Defines direction of movement.
	/// </summary>
	[SerializeField] private DirectionEnum verticalDirection;

	/// <summary>
	/// Defines multiple spawns of objects.
	/// </summary>
	private bool spawnMultiple { get; set; }

	private bool isActive = false;

	private void Awake()
	{
		Assert.IsTrue(objectToSpawn == PoolObjectKey.Spear || objectToSpawn == PoolObjectKey.BigSpear);
	}

	public void StartState(int damage, bool spawnMultiple)
	{
		this.spawnMultiple = spawnMultiple;
		this.damage = damage != 0 ? damage : this.damage;
		isActive = true;
		controller.SwapState(this);
	}

	public void StopState()
	{
		isActive = false;
		controller.EndState(this);
	}

	public override void OnEnter_State()
	{
		StartCoroutine(SpawnOffset());
	}

	private IEnumerator SpawnOffset()
	{
		yield return new WaitForSeconds(firstSpawnOffset);
		if (isActive)
		{
			do
			{
				SpawnGameObject();
				yield return new WaitForSeconds(spawnOffsetTime);
			} while (spawnMultiple && isActive);
		}

	}

	private void SpawnGameObject()
	{
		//if (ObjectToSpawn != null)
		//{
			Spear gameObject; //= Instantiate(ObjectToSpawn, transform);
			GameObject go = ObjectPooler.pooler.PullObject(objectToSpawn);
			if (go != null)
			{
				gameObject = go.GetComponent<Spear>();
				if (gameObject != null)
				{
					go.transform.position = transform.position;
					//go.transform.parent = transform;
					gameObject.SetValues(movementSpeed, damage, verticalDirection, horizontalDirection, objectToSpawn);
					go.SetActive(true);					
				}
				else
				{
					ObjectPooler.pooler.PushObject(go, objectToSpawn);
				}
			}
			//gameObject.SetValues(MovementSpeed, Damage, VerticalDirection, HorizontalDirection);
		//}
	}
}
