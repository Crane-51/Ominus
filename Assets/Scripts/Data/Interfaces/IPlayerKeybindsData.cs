using UnityEngine;

namespace Implementation.Data
{
    [DbContextConfiguration("PlayerKeybindsData")]
    public interface IPlayerKeybindsData : IUniqueIndex
    {
        /// <summary>
        /// Defines key for jumping.
        /// </summary>
        KeyCode KeyboardJump { get; set; }

        /// <summary>
        /// Defines key for going left.
        /// </summary>
        KeyCode KeyboardLeft { get; set; }

        /// <summary>
        /// Defines key for going right.
        /// </summary>
        KeyCode KeyboardRight { get; set; }

		/// <summary>
		/// Defines key for climbing a ledge.
		/// </summary>
		KeyCode KeyboardUp { get; set; }

		/// <summary>
		/// Defines key for dropping from a ledge.
		/// </summary>
		KeyCode KeyboardDown { get; set; }

		/// <summary>
		/// Gets or sets knockdown key.
		/// </summary>
		KeyCode KnockdownKey { get; set; }

        /// <summary>
        /// Gets or sets punch key.
        /// </summary>
        KeyCode KeyboardPunchKey { get; set; }

        /// <summary>
        /// Gets or sets spell action 1 key.
        /// </summary>
        KeyCode SpellAction1 { get; set; }

        /// <summary>
        /// Gets or sets stealth key.
        /// </summary>
        KeyCode KeyboardStealthKey { get; set; }

		/// <summary>
		/// Gets or sets crouch key.
		/// </summary>
		KeyCode KeyboardCrouchKey { get; set; }

		/// <summary>
		/// Gets or sets use key.
		/// </summary>
		KeyCode KeyboardUse { get; set; }

		/// <summary>
		/// Gets or sets inventory key.
		/// </summary>
		KeyCode KeyboardInventory { get; set; }

		/// <summary>
		/// Gets or sets dodge key.
		/// </summary>
		KeyCode KeyboardDodge { get; set; }

		/// <summary>
		/// Defines key for jumping.
		/// </summary>
		KeyCode JoystickJump { get; set; }

        /// <summary>
        /// Gets or sets punch key.
        /// </summary>
        KeyCode JoystickPunchKey { get; set; }


        /// <summary>
        /// Gets or sets stealth key.
        /// </summary>
        KeyCode JoystickStealthKey { get; set; }

        /// <summary>
        /// Gets or sets use key.
        /// </summary>
        KeyCode JoystickUse { get; set; }
    }
}
