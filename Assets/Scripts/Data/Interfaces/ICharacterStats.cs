namespace Implementation.Data
{
    [DbContextConfiguration("CharacterStats")]
    public interface ICharacterStats : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets character name.
        /// </summary>
      //  string Name { get; set; }

        /// <summary>
        /// Gets or sets character current health.
        /// </summary>
        int CurrentHealth { get; set; }

        /// <summary>
        /// Gets or sets character max health.
        /// </summary>
        int MaxHealth { get; set; }

		/// <summary>
		/// Gets or sets character strength
		/// </summary>
		int Strength { get; set; }

		/// <summary>
		/// Gets or sets character dexterity
		/// </summary>
		int Dexterity { get; set; }

		/// <summary>
		/// Adds health to character.
		/// </summary>
		/// <param name="health">The health points to add.</param>
		void AddHealth(int health, string id);

        /// <summary>
        /// Removes health from character, if it hits 0 it returns true else false.
        /// </summary>
        /// <param name="health">The health points to remove.</param>
        /// <returns>Value indicating weather health hit zero.</returns>
        bool RemoveHealth(int health, string id);
    }
}
