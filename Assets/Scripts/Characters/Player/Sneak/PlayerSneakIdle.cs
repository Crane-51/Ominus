using DiContainerLibrary.DiContainer;
using Implementation.Data;
using Player.Movement;
using UnityEngine;

namespace Player.Sneak
{
    public class PlayerSneakIdle : PlayerIdle
    {
        private const string HideObjectTag = "Hide";
        private const int orderInLayerNormal = 1001;
        private const int orderInLayerWhileHidden = 20;
        /// <summary>
        /// Gets or sets player key binds;
        /// </summary>
        [InjectDiContainter]
        private IPlayerKeybindsData keybinds;

        /// <summary>
        /// Gets or sets value indicating whether player can hide.
        /// </summary>
        private bool canHide { get; set; }

        /// <summary>
        /// Gets or sets sprite renderer.
        /// </summary>
        private SpriteRenderer spriteRenderer { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -9;
            keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
            canHide = false;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            spriteRenderer.sortingOrder = orderInLayerWhileHidden;
        }

        public override void Update_State()
        {
            if (controller.ActiveStateMovement != this && Input.GetKey(keybinds.KeyboardStealthKey) && canHide)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if((!Input.GetKey(keybinds.KeyboardStealthKey) && !Input.GetKey(keybinds.JoystickStealthKey)))
            {
                controller.EndState(this);
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            spriteRenderer.sortingOrder = orderInLayerNormal;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == HideObjectTag)
            {
                canHide = true;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == HideObjectTag)
            {
                canHide = false;
                controller.EndState(this);
            }
        }
    }
}
