using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Implementation.Data
{
    public class PhysicsOverlap : IPhysicsOverlap
    {
        public List<GameObject> Box(Transform point, float distance, float width = 1)
        {
            var result = Physics2D.OverlapBoxAll(new Vector2(point.position.x + (point.localScale.x), point.position.y - (point.transform.localScale.y/2)), new Vector2(distance, width), 0);

            DebugDrawBox(new Vector2(point.position.x + (point.localScale.x), point.position.y - (point.transform.localScale.y / 2)), new Vector2(distance, width), 0, Color.red, 10);

            return result.Select(x => x.gameObject).ToList();
        }

		public List<GameObject> Box(Vector2 point, float distance, float width = 1)
		{
			var result = Physics2D.OverlapBoxAll(new Vector2(point.x, point.y), new Vector2(distance, width), 0);

			DebugDrawBox(new Vector2(point.x, point.y), new Vector2(distance, width), 0, Color.red, 1);

			return result.Select(x => x.gameObject).ToList();
		}

		public List<GameObject> Circle(Transform point, float radius)
        {
            return Physics2D.OverlapCircleAll(point.position, radius).Select(x=> x.gameObject).ToList();
        }

        public static void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
        {
            var orientation = Quaternion.Euler(0, 0, angle);

            // Basis vectors, half the size in each direction from the center.
            Vector2 right = orientation * Vector2.right * size.x / 2f;
            Vector2 up = orientation * Vector2.up * size.y / 2f;

            // Four box corners.
            var topLeft = point + up - right;
            var topRight = point + up + right;
            var bottomRight = point - up + right;
            var bottomLeft = point - up - right;

            // Now we've reduced the problem to drawing lines.
            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }
    }
}
