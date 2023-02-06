using CustomCamera;
using Enviroment.Movement;
using UnityEngine;

public class MovingWallMovement : EnviromentMovement
{
    public override void WhileActive_State()
    {
        base.WhileActive_State();
        var offset = Vector2.Distance(gameInformation.Player.transform.position, transform.position);

        if(offset > 3)
        {
            ShakeCamera.singleton.StartShaking(3f);
        }
        else
        {
            ShakeCamera.singleton.StartShaking(8f);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        ShakeCamera.singleton.StopShaking();
    }
}
