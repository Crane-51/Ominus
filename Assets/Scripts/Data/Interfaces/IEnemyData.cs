using UnityEngine;

namespace Implementation.Data
{
    [DbContextConfiguration("EnemyData")]
    public interface IEnemyData : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets range of vision. 
        /// </summary>
        float RangeOfVision { get; set; }

		/// <summary>
		/// Gets of sets the angle of vision
		/// </summary>
		float AngleOfVision { get; set; }

        /// <summary>
        /// Gets or sets higher angle of vision.
        /// </summary>
        //float AngleOfVisionHigher { get; set; }

        ///// <summary>
        ///// Gets or sets lower angle of vision.
        ///// </summary>
        //float AngleOfVisionLower { get; set; }

        /// <summary>
        /// Gets or sets minimum range of the attack.
        /// </summary>
        float MinRangeOfAttack { get; set; }

		/// <summary>
		/// Gets or sets maximum range of the attack.
		/// </summary>
		float MaxRangeOfAttack { get; set; }

		/// <summary>
		/// Gets or sets value indicating weather enemy can attack player.
		/// </summary>
		bool CanAttack { get; set; }

        /// <summary>
        /// Gets or sets attack cool-down.
        /// </summary>
        float AttackCooldown { get; set; }

        /// <summary>
        /// Gets or sets enemy damage.
        /// </summary>
        //int Damage { get; set; }

		/// <summary>
		/// Displays is the player is visible to enemy.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		bool IsTargetDetected(Transform character, Transform target);

        /// <summary>
        /// Checks if target is in range of the melee attack;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool IsTargetInRangeOfMeleeAttack(Transform character, Transform target);

		/// <summary>
		/// Checks if target is in range of the ranged attack;
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		bool IsTargetInRangeOfRangedAttack(Transform character, Transform target);

		/// <summary>
		/// Checks if the target is close enough on x axis.
		/// </summary>
		/// <param name="ObjTransform">Position of the enemy.</param>
		/// <returns></returns>
		int LookAtTarget(Transform character, Transform target);

		/// <summary>
		/// Checks if the target is close enough on x axis.
		/// </summary>
		/// <param name="ObjTransform">Position of the enemy.</param>
		/// <returns></returns>
		int LookAtTarget(Transform character, Vector3 target);

		/// <summary>
		/// Look the opposite way.
		/// </summary>
		/// <param name="ObjTransform">Position of the enemy.</param>
		/// <returns></returns>
		int LookAwayFromTarget(Transform character, Transform target);

		/// <summary>
		/// Check if target is in range while not sneaking.
		/// </summary>
		/// <param name="character">Character that tracks.</param>
		/// <param name="target">Target.</param>
		/// <returns></returns>
		bool NotSneakingRangeOfDetection(Transform character, Transform target);

        /// <summary>
        /// Used when object is detected to check if its still in range of vision.
        /// </summary>
        /// <param name="position"> Position of object that requested function</param>
        /// <returns></returns>
        bool IsTargetStillInRangeOfVision(Transform character, Transform target);
    }
}