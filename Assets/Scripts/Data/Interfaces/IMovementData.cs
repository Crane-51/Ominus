namespace Implementation.Data
{
    [DbContextConfiguration("MovementData")]
    public interface IMovementData : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets movement speed.
        /// </summary>
        float MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets gravity equalizator.
        /// </summary>
        float GravityEqualizator { get; set; }

        /// <summary>
        /// Gets or sets gravity parameter.
        /// </summary>
        float Gravity { get; set; }

        /// <summary>
        /// Gets or sets jump height multiplicator.
        /// </summary>
        float JumpHeightMultiplicator { get; set; }

        /// <summary>
        /// Gets or sets flag for defining is character in air.
        /// </summary>
        //bool IsInAir { get; set; }

        /// <summary>
        /// Gets or sets horizontal movement parameter.
        /// </summary>
        float HorizontalMovement { get; set; }
		
		/// <summary>
        /// Gets or sets horizontal movement parameter.
        /// </summary>
        float VerticalMovement { get; set; }
    }
}
