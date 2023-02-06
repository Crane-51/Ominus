namespace Implementation.Data
{
    [DbContextConfiguration("CameraData")]
    public interface ICameraData : IUniqueIndex
    {
        /// <summary>
        /// Follow distance of the z axis.
        /// </summary>
        float FollowDistance { get; set; }

        /// <summary>
        /// Gets or sets movement speed of the camera.
        /// </summary>
        float MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets z axis offset.
        /// </summary>
        float ZAxisOffset { get; set; }

        /// <summary>
        /// Gets or sets y axis offset.
        /// </summary>
        float YAxisOffset { get; set; }
    }
}
