using UnityEngine;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="IMechanicsData"/>
    /// </summary>
    public class MechanicsData : IMechanicsData
    {
        /// <inheritdoc/>
        public GameObject spellPrefab { get; set; }

        /// <inheritdoc/>
        public GameObject spawnedObject { get; set; }
    }
}
