using System.Collections;
using General.State;
using UnityEngine;
using UnityEngine.Assertions;
using General.Enums;

public class RollingStoneSpawner : HighPriorityState
{
    /// <summary>
    /// Spawn multiple objects.
    /// </summary>
    [SerializeField] private bool spawnMultiple;

	/// <summary>
	/// Gets or sets first spawn offset.
	/// </summary>
	[SerializeField] private float firstSpawnOffset;

	/// <summary>
	/// Offset for multiple spawn
	/// </summary>
	[SerializeField] private float timeOffset;

	/// <summary>
	/// The object to spawn
	/// </summary>
	//public GameObject ObjectToSpawn;
	[SerializeField] private PoolObjectKey objectToSpawn;

	/// <summary>
	/// The damage of rolling stone.
	/// </summary>
	[SerializeField] private int damage;

	/// <summary>
	/// Speed of rolling
	/// </summary>
	[SerializeField] private float rollingForce;

	/// <summary>
	/// Max allowed force.
	/// </summary>
	[SerializeField] private float maxAllowedForce;

	/// <summary>
	/// Movement speed.
	/// </summary>
	[SerializeField] private float movementSpeed;

	private void Awake()
	{
		//Assert.IsNotNull(ObjectToSpawn);
		Assert.IsTrue(objectToSpawn == PoolObjectKey.DestroyStone || objectToSpawn == PoolObjectKey.StopStone);
	}

	protected override void Initialization_State()
    {
        base.Initialization_State();
        controller.SwapState(this);
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(firstSpawnOffset);
        do
        {
			//var spawnedObject = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity, transform).GetComponent<RollingStone>();
			RollingStone gameObject; //= Instantiate(ObjectToSpawn, transform);
			GameObject go = ObjectPooler.pooler.PullObject(objectToSpawn);
			if (go != null)
			{
				gameObject = go.GetComponent<RollingStone>();
				if (gameObject != null)
				{
					go.transform.position = transform.position;
					//go.transform.parent = transform;
					go.SetActive(true);
					gameObject.StartState(damage, rollingForce, maxAllowedForce, movementSpeed, objectToSpawn);
				}
				else
				{
					ObjectPooler.pooler.PushObject(go, objectToSpawn);
				}
			}

			//if (spawnedObject != null)
   //         {
   //             spawnedObject.StartState(Damage, RollingForce, MaxAllowedForce, MovementSpeed);
   //         }
            yield return new WaitForSeconds(timeOffset);
        } while (spawnMultiple);
    }
}
