using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using CustomCamera;

public class ShamanBossEnter : StateForMechanics
{
	[SerializeField] private EnemySharedDataAndInit shamanData;
	[SerializeField] private GameObject shaman;
	[InjectDiContainter] private IGameInformation gameInformation { get; set; }

	public void Activate()
	{
		StartCoroutine(EnterShaman());
	}

	private IEnumerator EnterShaman()
	{
		gameInformation.StopMovement = true;
		gameInformation.Camera.GetComponent<CameraFollowObject>().ActiveObjectToFollow = shaman.transform;
		yield return new WaitForSeconds(3);
		gameInformation.Camera.GetComponent<CameraFollowObject>().ActiveObjectToFollow = gameInformation.Player.transform;
		gameInformation.StopMovement = false;
		shamanData.targetLocked = true;
		shamanData.enemyData.CanAttack = true;
	}
}
