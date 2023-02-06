using System.Collections.Generic;
using UnityEngine;

namespace Implementation.Data
{
    public interface IPhysicsOverlap
    {
        /// <summary>
        /// Detects all enemies in the circle of the object.
        /// </summary>
        /// <returns>List of game objects in sphere.</returns>
        List<GameObject> Circle(Transform point, float radius);

        /// <summary>
        /// Detects all enemies in the box of the object.
        /// </summary>
        /// <returns>List of game objects in box.</returns>
        List<GameObject> Box(Transform point, float radius, float width = 1);
		List<GameObject> Box(Vector2 point, float length, float width = 1);
    }
}
