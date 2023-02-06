using UnityEngine;
using System;

namespace Implementation.Data
{
    [Serializable]
    public class PlayerKeybindsData : IPlayerKeybindsData
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public KeyCode KeyboardJump { get { return keyboardJump; } set { keyboardJump = value; } }
		public KeyCode keyboardJump;

        /// <inheritdoc />
        public KeyCode KeyboardLeft { get { return keyboardLeft; } set { keyboardLeft = value; } }
		public KeyCode keyboardLeft;

        /// <inheritdoc />
        public KeyCode KeyboardRight { get { return keyboardRight; } set { keyboardRight = value; } }
		public KeyCode keyboardRight;

		/// <inheritdoc />
		public KeyCode KeyboardUp { get { return keyboardUp; } set { keyboardUp = value; } }
		public KeyCode keyboardUp;

		/// <inheritdoc />
		public KeyCode KeyboardDown { get { return keyboardDown; } set { keyboardDown = value; } }
		public KeyCode keyboardDown;

		/// <inheritdoc />
		public KeyCode KnockdownKey { get; set; }

        /// <inheritdoc />
        public KeyCode KeyboardPunchKey { get { return keyboardPunchKey; } set { keyboardPunchKey = value; } }
		public KeyCode keyboardPunchKey;

        /// <inheritdoc />
        public KeyCode SpellAction1 { get; set; }

        /// <inheritdoc />
        public KeyCode KeyboardStealthKey { get { return keyboardStealthKey; } set { keyboardStealthKey = value; } }
		public KeyCode keyboardStealthKey;
		
		/// <inheritdoc />
        public KeyCode KeyboardCrouchKey { get { return keyboardCrouchKey; } set { keyboardCrouchKey = value; } }
		public KeyCode keyboardCrouchKey;

        /// <inheritdoc />
        public KeyCode KeyboardUse { get { return keyboardUse; } set { keyboardUse = value; } }
		public KeyCode keyboardUse;
		
		/// <inheritdoc />
        public KeyCode KeyboardInventory { get { return keyboardInventory; } set { keyboardInventory = value; } }
		public KeyCode keyboardInventory;

		/// <inheritdoc />
		public KeyCode KeyboardDodge { get { return keyboardDodge; } set { keyboardDodge = value; } }
		public KeyCode keyboardDodge;

        /// <inheritdoc />
        public KeyCode JoystickJump { get; set; }

        /// <inheritdoc />
        public KeyCode JoystickPunchKey { get; set; }

        /// <inheritdoc />
        public KeyCode JoystickStealthKey { get; set; }

        /// <inheritdoc />
        public KeyCode JoystickUse { get; set; }
    }
}
