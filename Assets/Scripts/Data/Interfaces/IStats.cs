using General.Enums;

namespace Implementation.Data
{
    [DbContextConfiguration("Stats")]
    public interface IStats : ICharacterStats
    {
        /// <summary>
        /// Gets or sets player mana points.
        /// </summary>
        int CurrentMana { get; set; }

        /// <summary>
        /// Gets or sets max player mana points.
        /// </summary>
        int MaxMana { get; set; }

        /// <summary>
        /// Gets or sets imagination points.
        /// </summary>
        int CurrentImagination { get; set; }

        /// <summary>
        /// Gets or sets max player imagination points.
        /// </summary>
        int MaxImagination { get; set; }

		/// <summary>
		/// Gets or sets the weapon used.
		/// </summary>
		WeaponId Weapon { get; set; }

        /// <summary>
        /// Adds mana point.
        /// </summary>
        /// <param name="mana">Mana points to add.</param>
        void AddMana(int mana, string id);

        /// <summary>
        /// Removes mana points.
        /// </summary>
        /// <param name="mana">Points to remove</param>
        /// <returns>Value that indicates weather there is enough points for spending</returns>
        bool RemoveMana(int mana, string id);

        /// <summary>
        /// Adds imagination points.
        /// </summary>
        /// <param name="imagination">Imagination points to add.</param>
        void AddImagination(int imagination, string id);

        /// <summary>
        /// Removes imagination points
        /// </summary>
        /// <param name="imagination">Imagination points to remove.</param>
        /// <returns>Value indicating weather there is enough points for spending</returns>
        bool RemoveImagination(int imagination, string id);
    }
}
