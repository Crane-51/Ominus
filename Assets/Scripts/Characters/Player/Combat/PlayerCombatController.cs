using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using Player.Movement;
using Player.Sneak;
using Character.Stats;

namespace Player.Mechanic.Combat
{
    public class PlayerCombatController : StateForMechanics
    {
        /// <summary>
        /// Gets or sets key binds.
        /// </summary>
        [InjectDiContainter]
        private IPlayerKeybindsData keyBinds { get; set; }

        /// <summary>
        /// Defines Combo.
        /// </summary>
        [SerializeField] private List<State> attackStates;

        /// <summary>
        /// Gets or sets combo index.
        /// </summary>
        private int ComboIndex { get; set; }

        /// <summary>
        /// Gets or sets last active state for movement.
        /// </summary>
        private StateForMovement lastStateForMovement { get; set; }
		private EquipmentManager equipManager;

		[SerializeField] private int sneakAttackBonus = 10;
		private int sneakBonusToApply = 0;
		public int SneakBonusToApply { get { return sneakBonusToApply; } }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            keyBinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
            Priority = 15;
            ComboIndex = 0;
			equipManager = GetComponent<EquipmentManager>();
        }

        public override void OnEnter_State()
        {
            if(!(lastStateForMovement is PlayerCombatMovement))
            {
                ComboIndex = 0;
            }
			if (lastStateForMovement is PlayerIdle || lastStateForMovement is PlayerSneak || lastStateForMovement is PlayerSneakIdle)
			{
				sneakBonusToApply = sneakAttackBonus;
			}
			else
			{
				sneakBonusToApply = 0;
			}
            controller.SwapState(attackStates[ComboIndex % attackStates.Count]);
            ComboIndex++;
        }

        public override void Update_State()
        {
            base.Update_State();

			if (controller.ToString() != "")
			{
				lastStateForMovement = controller.ActiveStateMovement;

			}
			if (Input.GetKeyDown(keyBinds.KeyboardPunchKey) && equipManager.EquippedItem != null && equipManager.EquippedItem.name == "Fists" 
				&& controller.ActiveStateMechanic != this && !(lastStateForMovement is PlayerGrabLedge))
            {				
				controller.SwapState(this);
            }
        }

        public override void OnExit_State()
        {
        }
    }
}
