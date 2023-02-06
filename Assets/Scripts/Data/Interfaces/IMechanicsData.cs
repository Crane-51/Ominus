using UnityEngine;

namespace Implementation.Data
{
    public interface IMechanicsData
    {
        /// <summary>
        /// Gets or sets spell prefab.
        /// </summary>
        GameObject spellPrefab { get; set; }

        /// <summary>
        /// Gets or sets instantieted object.
        /// </summary>
        GameObject spawnedObject { get; set; }
    }
}
