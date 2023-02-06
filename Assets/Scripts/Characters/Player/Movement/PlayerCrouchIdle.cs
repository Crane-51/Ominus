using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;

namespace Assets.Scripts.Characters.Player.Movement
{
    public class PlayerCrouchIdle : PlayerCrouchMovement
    {
		//[InjectDiContainter]
		//protected IPlayerKeybindsData keybinds;

		protected override void Initialization_State()
        {
            base.Initialization_State();
			//Priority = 30;
			Priority = -9;
			//keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        public override void Update_State()
        {
            if (Input.GetKey(keybinds.KeyboardCrouchKey))
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            if (!Input.GetKey(keybinds.KeyboardCrouchKey) && hit.transform == null)
            {
                controller.EndState(this);
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            rigBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
