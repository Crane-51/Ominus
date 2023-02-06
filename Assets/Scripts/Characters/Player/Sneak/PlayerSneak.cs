using Implementation.Data;
using Player.Movement;
using UnityEngine;

namespace Player.Sneak
{
    public class PlayerSneak : PlayerMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -4;
            keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            MovementData.MovementSpeed /= 2;
        }

        public override void Update_State()
        {
            if(!Input.GetKey(keybinds.KeyboardStealthKey))
            {
                return;
            }
            base.Update_State();
        }

        public override void WhileActive_State()
        {
            if (!Input.GetKey(keybinds.KeyboardStealthKey))
            {
                controller.EndState(this);
                return;
            }
            base.WhileActive_State();
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            MovementData.MovementSpeed *= 2;
        }
    }
}
