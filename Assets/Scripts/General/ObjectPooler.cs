using System.Collections.Generic;
using UnityEngine;
using General.Enums;

[System.Serializable]
public class Pool
{
	[HideInInspector] public List<GameObject> inactiveObjectPool;
	[HideInInspector] public List<GameObject> activeObjectPool;
	[SerializeField] public PoolObjectKey key;
	[SerializeField] public GameObject prefab;
	[SerializeField] public int initialPoolSize;
}

public class ObjectPooler : MonoBehaviour
{
	[HideInInspector] public static ObjectPooler pooler;

	[SerializeField] private List<Pool> poolList;
	private Dictionary<PoolObjectKey, Pool> pools;

	private void Awake()
	{
		pooler = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		pools = new Dictionary<PoolObjectKey, Pool>();
		foreach (Pool pool in poolList)
		{
			pool.activeObjectPool = new List<GameObject>();
			pool.inactiveObjectPool = new List<GameObject>();
			for (int i = 0; i < pool.initialPoolSize; i++)
			{
				GameObject go = Instantiate(pool.prefab);
				go.SetActive(false);
				pool.inactiveObjectPool.Add(go);
			}
			pools.Add(pool.key, pool);
		}
		poolList.Clear();
    }

    public GameObject PullObject(PoolObjectKey key)
	{
		Pool pool = pools[key];
		if (pool != null)
		{
			GameObject go;
			if (pool.inactiveObjectPool.Count == 0)
			{
				go = Instantiate(pool.prefab);
				go.SetActive(false);
			}
			else
			{
				go = pool.inactiveObjectPool[0];
				pool.inactiveObjectPool.RemoveAt(0);
			}

			pool.activeObjectPool.Add(go);
			return go;
		}

		return null;
	}

	public void PushObject(GameObject go, PoolObjectKey key)
	{
		Pool pool = pools[key];
		if (pool != null)
		{
			go.SetActive(false);
			pool.activeObjectPool.Remove(go);
			pool.inactiveObjectPool.Add(go);
		}
	}

}
