using UnityEngine;
using Character.Stats;
using General.State;
using General.Enums;

public class DmgAndDestroyOnCollision : MonoBehaviour
{
	[HideInInspector]
	public int dmg;
	[HideInInspector] public string dmgTag;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == dmgTag)
		{
			int direction = 0;
			float absAngle = Mathf.Abs(transform.rotation.eulerAngles.z) % 360;
			if ((absAngle >= 0 && absAngle <= 90) || (absAngle >= 270 && absAngle <= 360))
			{
				direction = 1;
			}
			else
			{
				direction = -1;
			}
			Debug.Log(dmgTag + " " + dmg);
			collision.gameObject.GetComponent<CharacterTakeDamage>().TakeDamage(dmg, direction, 1, true);
		}
		//controller.ActiveHighPriorityState is EnemyWakeUp || controller.ActiveStateMovement is EnemySleep
		if ((collision.tag == dmgTag && !(collision.GetComponent<StateController>().ActiveStateMovement is EnemySleep))/*|| collision.tag == "Ground"*/ || collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
		{
			//Destroy(gameObject);
			//gameObject.SetActive(false);
			ObjectPooler.pooler.PushObject(gameObject, PoolObjectKey.Arrow);
		}
	}
}
