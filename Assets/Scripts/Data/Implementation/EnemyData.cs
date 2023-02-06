using System;
using System.Linq;
using UnityEngine;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="IEnemyData"/>.
    /// </summary>
    [Serializable]
    public class EnemyData : IEnemyData
	{
        /// <inheritdoc />
        public string Id { get; set; }
		//public string id;

        /// <inheritdoc/>
        public float RangeOfVision { get { return rangeOfVision; } set { rangeOfVision = value; } }
		public float rangeOfVision;

		/// <inheritdoc />
		public float AngleOfVision { get { return angleOfVision; } set { angleOfVision = value; } }
		public float angleOfVision;

		///// <inheritdoc/>
		//public float AngleOfVisionHigher { get; set; }
		//public float angleOfVisionHigher;

  //      /// <inheritdoc/>
  //      public float AngleOfVisionLower { get; set; }
		//public float angleOfVisionLower;

        /// <inheritdoc/>
        public float MinRangeOfAttack { get { return minRangeOfAttack; } set { minRangeOfAttack = value; } }
		public float minRangeOfAttack;

		/// <inheritdoc/>
		public float MaxRangeOfAttack { get { return maxRangeOfAttack; } set { maxRangeOfAttack = value; } }
		public float maxRangeOfAttack;

		/// <inheritdoc/>
		public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
		public bool canAttack;

        /// <inheritdoc/>
        public float AttackCooldown { get { return attackCooldown; } set { attackCooldown = value; } }
		public float attackCooldown;

		///// <inheritdoc/>
		//public int Damage { get; set; }
		//public int damage;

        /// <inheritdoc/>
        public bool IsTargetDetected(Transform character, Transform target)
        {
            var result = Physics2D.OverlapBoxAll(new Vector2(character.position.x + (character.localScale.x * RangeOfVision / 2), character.position.y), new Vector2(RangeOfVision *character.transform.localScale.x, character.transform.localScale.y), 0);
            return result.Select(x => x.gameObject).ToList().Contains(target.gameObject);
        }

        /// <inheritdoc/>
        public bool NotSneakingRangeOfDetection(Transform character, Transform target)
        {
			RaycastHit2D hit;
			hit = Physics2D.Raycast(character.transform.position, character.transform.right * character.transform.localScale.x, RangeOfVision/3, LayerMask.GetMask("Player", "Environment", "Border"));
			Debug.DrawRay(character.transform.position, character.transform.right * character.transform.localScale.x * RangeOfVision/3, Color.green);
			if (hit.transform != null && hit.transform.gameObject.tag == target.transform.tag)
			{
				return true;
			}
			else
			{
				return false;
			}
			//if (Physics2D.OverlapCircleAll(character.position, RangeOfVision/3).Select(x => x.gameObject).Contains(target.gameObject))
			//         {
			//             return true;
			//         }

			//         return false;
		}

        /// <inheritdoc/>
        public bool IsTargetStillInRangeOfVision(Transform character, Transform target)
        {
			//Debug.DrawRay(character.transform.position, (target.transform.position - character.transform.position).normalized * RangeOfVision, Color.cyan);
			//if (Physics2D.OverlapCircleAll(character.position, RangeOfVision).Select(x => x.gameObject).Contains(target.gameObject))
			//         {				
			RaycastHit2D hit;
			Vector2 direction;
			int playerDir = 1;
			playerDir = target.position.x - character.position.x > 0 ? 1 : -1;
			if (target.transform.tag == "Player")
			{
				direction = (target.transform.position - Vector3.up / 2f - character.transform.position).normalized;
				if (Vector2.SignedAngle(character.transform.right * playerDir, direction) > AngleOfVision)
				{
					direction = Quaternion.AngleAxis(AngleOfVision, character.transform.forward) * character.transform.right * playerDir;

				}
				else if (Vector2.SignedAngle(character.transform.right * playerDir, direction) < -AngleOfVision)
				{
					direction = Quaternion.AngleAxis(-AngleOfVision, character.transform.forward) * character.transform.right * playerDir;
				}

				if (Id == "SkeletonArcher")
				{
					hit = Physics2D.Raycast(character.transform.position, direction, RangeOfVision, LayerMask.GetMask("Player", "Environment"));
				}
				else
				{
					hit = Physics2D.Raycast(character.transform.position, direction, RangeOfVision, LayerMask.GetMask("Player", "Environment", "Border"));
				}

				Debug.DrawRay(character.transform.position, direction * RangeOfVision, Color.white);
			}
			else
			{
				hit = Physics2D.Raycast(character.transform.position, target.transform.position - character.transform.position, RangeOfVision, LayerMask.GetMask("Player", "Environment", "Border"));
				//Debug.DrawRay(character.transform.position, (target.transform.position - character.transform.position).normalized * RangeOfVision, Color.white);
			}

                if (hit.transform != null && hit.transform.gameObject.tag == target.transform.tag)
                {
                    return true;
                }
            //}

            return false;
        }

        /// <inheritdoc/>
        public bool IsTargetInRangeOfMeleeAttack(Transform character, Transform target)
        {
			//if (Physics2D.OverlapCircleAll(character.position, MinRangeOfAttack).Select(x => x.gameObject).Contains(target.gameObject))
			//if (Vector2.Distance(character.position, target.position) <= MinRangeOfAttack+1)
			RaycastHit2D hit;
			hit = Physics2D.Raycast(character.transform.position, target.transform.position - Vector3.up / 2f - character.transform.position, MinRangeOfAttack, LayerMask.GetMask("Player", "Environment", "Border"));
			Debug.DrawRay(character.transform.position, (target.transform.position - character.transform.position).normalized * (MinRangeOfAttack), Color.cyan);
			if (hit.transform != null && hit.transform.gameObject.tag == target.transform.tag)
            {
                return true;
            }
            return false;
        }

		/// <inheritdoc/>
		public bool IsTargetInRangeOfRangedAttack(Transform character, Transform target)
		{
			bool condi1 = Physics2D.OverlapCircleAll(character.position, MaxRangeOfAttack).Select(x => x.gameObject).Contains(target.gameObject);
			bool condi2 = !Physics2D.OverlapCircleAll(character.position, MinRangeOfAttack - 0.7f).Select(x => x.gameObject).Contains(target.gameObject);
			if (condi1 && condi2)
			{
				return true;
			}
			return false;
		}

		/// <inheritdoc/>
		public int LookAtTarget(Transform character, Transform target)
        {
            var roationAngle = target.position.x - character.position.x;

            //// Proper rotation
            if (roationAngle < 0)
            {
                character.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                character.localScale = new Vector3(1, 1, 1);
            }

            if (Vector2.Distance(new Vector2(target.transform.position.x, 0), new Vector2(character.transform.position.x, 0)) < MinRangeOfAttack / 2)
            {
                return 0;
            }

            return roationAngle > 0 ? 1 : -1;
        }

		/// <inheritdoc/>
		public int LookAtTarget(Transform character, Vector3 target)
		{
			var roationAngle = target.x - character.position.x;

			//// Proper rotation
			if (roationAngle < 0)
			{
				character.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				character.localScale = new Vector3(1, 1, 1);
			}

			//if (Vector2.Distance(new Vector2(target.x, 0), new Vector2(character.transform.position.x, 0)) < MinRangeOfAttack / 2)
			//{
			//	return 0;
			//}

			return roationAngle > 0 ? 1 : -1;
		}

		/// <inheritdoc/>
		public int LookAwayFromTarget(Transform character, Transform target)
		{
			var roationAngle = character.position.x - target.position.x;

			//// Proper rotation
			if (roationAngle < 0)
			{
				character.localScale = new Vector3(-1, 1, 1);
			}
			else
			{
				character.localScale = new Vector3(1, 1, 1);
			}

			return roationAngle > 0 ? 1 : -1;
		}
	}
}
