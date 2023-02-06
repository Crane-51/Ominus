using General.State;
using Player.Movement;
using UnityEngine;
using System.Collections.Generic;
using Character.Stats;
using System.Collections;

namespace Player.Other
{
    public class PlayerGravity : StateForMovement
    {
        /// <summary>
        /// Defines tag that enables player to jump.
        /// </summary>
        public const string ObjectsThatEnableJump = "Ground";

		private static Dictionary<string, bool> jumpEnablers;

        /// <summary>
        /// Gets or sets object collision count.
        /// </summary>
		// public static int CollisionCount { get; set; }
        public static bool IsGrounded { get; set; }
		public static bool GravityEnabled { get; set; } = true;
		private static float startPos;
		private static float endPos;
		[SerializeField] private float dmgThreshold = 7f;
		[SerializeField] private int startingDmg = 5;
		private static float _dmgThreshold = 7f;
		private static int _startingDmg = 5;
		[SerializeField] private float deathHeight = 20f;
		private static float dmgFactor = 1f;
		private static CharacterTakeDamage charDmg;
		private static Transform trans;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			jumpEnablers = new Dictionary<string, bool>();
			jumpEnablers.Add("Ground", false);
			jumpEnablers.Add("Climbable", false);
			jumpEnablers.Add("Swinging", false);
			charDmg = GetComponent<CharacterTakeDamage>();
			trans = GetComponent<Transform>();
			startPos = transform.position.y;
			endPos = startPos;
			_dmgThreshold = dmgThreshold;
			_startingDmg = startingDmg;
			StartCoroutine(WaitForInit());
			//dmgFactor = (GetComponent<CharacterStatsMono>().MaxHealth - startingDmg) / (deathHeight - dmgThreshold);
			//Debug.Log("Max health: " + GetComponent<CharacterStatsMono>().MaxHealth);
			//Debug.Log("startingDmg: " + startingDmg);
			//Debug.Log("death height: " + deathHeight);
			//Debug.Log("dmgThreshold: " + dmgThreshold);
			//Debug.Log("Res: "+ dmgFactor);
		}

		public override void Update_State()
		{
			//if (rigBody.bodyType != RigidbodyType2D.Static)
			if (GravityEnabled && rigBody.bodyType != RigidbodyType2D.Static)
			{
				rigBody.velocity -= new Vector2(0, MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime);
			}

			//ContactPoint2D[] contacts = new ContactPoint2D[10];
			//int contactsCount = rigBody.GetContacts(contacts);
			//foreach (var contact in contacts)
			//{
			//	if (contact.normalImpulse > 0.9f && contact.normalImpulse < 1f)
			//	{
			//		rigBody.velocity = new Vector2(rigBody.velocity.x, -10);
			//	}
			//}

			if (/*!IsGrounded &&*/ !(controller.ActiveStateMovement is PlayerJump || controller.ActiveStateMovement is PlayerSwinging || controller.ActiveStateMovement is PlayerSwingingIdle) 
				&& rigBody.velocity.y > 0)
			{
				rigBody.velocity = new Vector2(rigBody.velocity.x, -10);

			}
		}

		public static void GroundedTrigger(string tag, bool triggerEntered)
		{
			if (!IsGrounded && triggerEntered)
			{
				endPos = trans.position.y;
				float diffPos = Mathf.Abs(endPos - startPos);
				if (diffPos > _dmgThreshold)
				{
					int dmg = 20;
					dmg = Mathf.RoundToInt((diffPos - _dmgThreshold) * dmgFactor + _startingDmg);
					charDmg.TakeDamage(dmg);
					Debug.Log("Diff in pos: " + diffPos);
					Debug.Log("dmg: " + dmg);
				}
			}
			else
			{
				startPos = trans.position.y;
			}

			if (jumpEnablers.ContainsKey(tag))
			{
				jumpEnablers[tag] = triggerEntered;
			}

			IsGrounded = jumpEnablers.ContainsValue(true);
		}

		public static bool TagIsGroundedTrigger(string tag)
		{
			return jumpEnablers.ContainsKey(tag);
		}

		//private void OnTriggerExit2D(Collider2D collision)
		//{
		//	if (TagIsGroundedTrigger(collision.tag))
		//	{
		//		startPos = transform.position.y;
		//	}
		//}

		//private void OnTriggerEnter2D(Collider2D collision)
		//{
		//	if (TagIsGroundedTrigger(collision.tag))
		//	{
		//		endPos = transform.position.y;
		//		float diffPos = Mathf.Abs(endPos - startPos);
		//		Debug.Log("Diff in pos: " + diffPos);
		//		if (diffPos > dmgThreshold)
		//		{
		//			int dmg = 20;
		//			dmg = Mathf.RoundToInt((diffPos - dmgThreshold) * dmgFactor + startingDmg);
		//			charDmg.TakeDamage(dmg);
		//			Debug.Log("Diff in pos: " + diffPos);
		//			Debug.Log("dmg: " + dmg);
		//		}
		//	}
		//}

		private IEnumerator WaitForInit()
		{
			yield return null;
			dmgFactor = (GetComponent<CharacterStatsMono>().MaxHealth - _startingDmg) / (deathHeight - _dmgThreshold);
			//Debug.Log("Max health: " + GetComponent<CharacterStatsMono>().MaxHealth);
			//Debug.Log("startingDmg: " + _startingDmg);
			//Debug.Log("death height: " + deathHeight);
			//Debug.Log("dmgThreshold: " + _dmgThreshold);
			//Debug.Log("Res: " + dmgFactor);

		}

		//private void FixedUpdate()
		//{
		//	if (rigBody.bodyType != RigidbodyType2D.Static)
		//	{
		//		rigBody.velocity -= new Vector2(0, MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime);
		//	}

		//	if (/*!IsGrounded &&*/ !(controller.ActiveStateMovement is PlayerJump) && rigBody.velocity.y > 0)
		//	{
		//		rigBody.velocity = new Vector2(rigBody.velocity.x, -10);
		//		//ContactPoint2D[] contacts = new ContactPoint2D[10];
		//		//int contactsCount = rigBody.GetContacts(contacts);
		//		//foreach (var contact in contacts)
		//		//{
		//		//	if (contact.normalImpulse > 0.9f && contact.normalImpulse < 1f)
		//		//	{
		//		//		rigBody.velocity = new Vector2(rigBody.velocity.x, -10);
		//		//	}
		//		//}
		//	}
		//}
	}
}
