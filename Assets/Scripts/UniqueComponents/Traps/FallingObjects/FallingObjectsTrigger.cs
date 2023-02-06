using System.Collections.Generic;
using General.State;
using UnityEngine;

namespace UniqueComponent.FallingObject
{
    public class FallingObjectsTrigger : HighPriorityState
    {
        public List<FallingObject> objectsThatWillFall;

        public override void OnEnter_State()
        {
            base.OnEnter_State();

            foreach (var item in objectsThatWillFall)
            {
                item.IsActivated();
            }
            Destroy(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                controller.SwapState(this);
            }
        }
    }
}

