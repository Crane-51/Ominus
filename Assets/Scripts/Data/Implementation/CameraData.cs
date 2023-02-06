using System;

namespace Implementation.Data
{
    [Serializable]
    public class CameraData : ICameraData
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public float FollowDistance { get; set; }

        /// <inheritdoc/>
        public float MovementSpeed { get; set; }

        /// <inheritdoc/>
        public float ZAxisOffset { get; set; }

        /// <inheritdoc/>
        public float YAxisOffset { get; set; }
    }
}
