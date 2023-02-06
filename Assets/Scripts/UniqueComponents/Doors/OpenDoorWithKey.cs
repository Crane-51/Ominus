using General.State;
using Implementation.Custom;
using UnityEngine;

namespace UniqueComponent.Doors
{
    public class OpenDoorWithKey : HighPriorityState, IActivate
    {
        private Collider2D coll2D { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            coll2D = GetComponent<Collider2D>();
            Priority = 1;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            coll2D.enabled = false;
        }

        public void Activate()
        {
            controller.SwapState(this);
        }
    }
}
