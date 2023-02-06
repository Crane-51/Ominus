using Enviroment.Movement;
using UnityEngine;

namespace UniqueComponent.Platform
{
    public class PlatformMovement : EnviromentMovement
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (gameInformation == null) return;
            if (other.gameObject == gameInformation.Player)
            {
                other.collider.transform.SetParent(transform);
            }
            else
            {
                convertedDirection *= -1;
            }
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject == gameInformation.Player)
            {
                other.collider.transform.SetParent(null);
            }
        }
    }
}
