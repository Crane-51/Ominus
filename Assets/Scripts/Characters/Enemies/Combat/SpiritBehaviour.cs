using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stats;

public class SpiritBehaviour : MonoBehaviour
{
	[HideInInspector] public int dmg;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.gameObject.GetComponent<CharacterTakeDamage>().TakeDamage(dmg, transform.right.x, 1, true);
			/*
			CharTakeDmg.OnEnter_State:
			-rm base.OnEnter_state()
			- play sound
			- play animation

			|| new player state
			*/
		}
		else if (collision.tag == "Ground") // or border?
		{
			Destroy(gameObject);
		}
	}
}
