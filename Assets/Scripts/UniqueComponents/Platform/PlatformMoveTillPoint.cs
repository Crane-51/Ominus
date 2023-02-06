using Enviroment.Movement;

namespace UniqueComponent.Platform
{
    public class PlatformMoveTillPoint : EnviromentMovement
    {
        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if(collision.gameObject.tag == "Border")
            {
                controller.EndState(this);
            }
        }
    }
}
