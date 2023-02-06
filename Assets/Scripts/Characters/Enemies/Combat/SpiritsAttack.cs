using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;

public class SpiritsAttack : AttackSkill
{
	//[SerializeField] private int dmg;
	//[SerializeField] private float cooldown;
	//[SerializeField] private float castingTime;
	[SerializeField] private GameObject spiritsPrefab;
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private float spiritsSpeed;
	//private EnemySharedDataAndInit sharedData;
	//private float invokingTime;

	//protected override void Initialization_State()
	//{
	//	base.Initialization_State();
	//	Priority = 5; //check
	//	sharedData = GetComponent<EnemySharedDataAndInit>();
	//	invokingTime = designController.animationController.GetAnimationClipLength("Invoking");
	//}

	//public override void OnEnter_State()
	//{
	//	base.OnEnter_State();
	//	sharedData.enemyData.CanAttack = false;
	//	StartCoroutine(StartCasting());
	//}

	//private IEnumerator StartCasting()
	//{
	//	//start chanting animation
	//	designController.animationController.Anima.SetBool("Chanting", true);
	//	//wait for castingtime seconds
	//	yield return new WaitForSeconds(castingTime);
	//	//end chanting animation
	//	designController.animationController.Anima.SetBool("Chanting", false);
	//	//start invoking animation
	//	designController.animationController.Anima.SetBool("Invoking", true);
	//	//spawn horses & give them velocity
	//	yield return new WaitForSeconds(invokingTime);
	//	GameObject spirits = Instantiate(spiritsPrefab, bulletSpawn.position, bulletSpawn.rotation);
	//	spirits.transform.localScale = new Vector3(spirits.transform.localScale.x * transform.localScale.x, spirits.transform.localScale.y);
	//	Rigidbody2D rb = spirits.GetComponent<Rigidbody2D>();
	//	spirits.GetComponent<SpiritBehaviour>().dmg = dmg;
	//	rb.velocity = new Vector2(spiritsSpeed * transform.localScale.x, 0f);
	//	//end invoking animation
	//	designController.animationController.Anima.SetBool("Invoking", false);
	//	//start cooldown
	//	controller.EndState(this);
	//	yield return new WaitForSeconds(cooldown);
	//	sharedData.enemyData.CanAttack = true;

	//}

	protected override IEnumerator AttackImplementation()
	{
		GameObject spirits = Instantiate(spiritsPrefab, bulletSpawn.position, bulletSpawn.rotation);
		spirits.transform.localScale = new Vector3(spirits.transform.localScale.x * transform.localScale.x, spirits.transform.localScale.y);
		Rigidbody2D rb = spirits.GetComponent<Rigidbody2D>();
		spirits.GetComponent<SpiritBehaviour>().dmg = dmg;
		rb.velocity = new Vector2(spiritsSpeed * transform.localScale.x, 0f);
		yield return null;
	}
}
