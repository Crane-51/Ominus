using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Implementation.Data
{
    /// <summary>
    /// Contains implementation of <see cref="IMovementData"/>.
    /// </summary>
    [Serializable]
    public class MovementData : IMovementData
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc/>
        public float HorizontalMovement { get; set; }
		//public float horizontalMovement;

		/// <inheritdoc/>
		public float VerticalMovement { get; set; }

        /// <inheritdoc/>
        public float GravityEqualizator { get { return gravityEqualizator; } set { gravityEqualizator = value; } }
		public float gravityEqualizator;

        /// <inheritdoc/>
        public float Gravity { get { return gravity; } set { gravity = value; } }
		public float gravity;

        /// <inheritdoc/>
        public float JumpHeightMultiplicator { get { return jumpHeightMultiplicator; } set { jumpHeightMultiplicator = value; } }
		public float jumpHeightMultiplicator;

        /// <inheritdoc/>
  //      public bool IsInAir { get; set; }
		//public float isInAir;

        /// <inheritdoc/>
        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
		public float movementSpeed;
    }
}
