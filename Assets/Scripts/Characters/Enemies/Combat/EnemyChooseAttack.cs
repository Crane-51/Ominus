using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;

public class EnemyChooseAttack : StateForMechanics
{
	private EnemySharedDataAndInit sharedData;
	[SerializeField] List<AttackSkill> attackList;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		//Priority = ?;
		sharedData = GetComponent<EnemySharedDataAndInit>();
	}

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMechanic != this && sharedData.enemyData.CanAttack && sharedData.targetLocked)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		controller.SwapState(attackList[0]);
	}
}
