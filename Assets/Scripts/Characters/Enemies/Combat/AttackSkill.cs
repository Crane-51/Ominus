using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;

public abstract class AttackSkill : StateForMechanics
{
	[SerializeField] protected int dmg;
	[SerializeField] protected float cooldown;
	[SerializeField] protected float castingTime;
	protected EnemySharedDataAndInit sharedData;
	protected float attackTime;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = 5; //check
		sharedData = GetComponent<EnemySharedDataAndInit>();
		attackTime = designController.animationController.GetAnimationClipLength("Invoking");//check when multiple animations will be present
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		sharedData.enemyData.CanAttack = false; //make it per attack? or like this, general?
		StartCoroutine(StartCasting());
	}

	protected IEnumerator StartCasting()
	{
		designController.animationController.Anima.SetBool("Chanting", true);
		yield return new WaitForSeconds(castingTime);
		designController.animationController.Anima.SetBool("Chanting", false);
		StartCoroutine(Attack());
	}

	protected IEnumerator Attack()
	{
		designController.animationController.Anima.SetBool("Invoking", true);	//maybe leave it to the implementation?
		yield return new WaitForSeconds(attackTime);
		designController.animationController.Anima.SetBool("Invoking", false);
		//here goes attack and wait for over
		yield return StartCoroutine(AttackImplementation());
		StartCoroutine(StartCooldown());
	}

	protected IEnumerator StartCooldown()
	{
		controller.EndState(this);
		yield return new WaitForSeconds(cooldown);
		sharedData.enemyData.CanAttack = true;
	}

	protected abstract IEnumerator AttackImplementation();
}
