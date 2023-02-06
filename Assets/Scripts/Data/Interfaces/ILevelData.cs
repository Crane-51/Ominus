namespace Implementation.Data
{
    [DbContextConfiguration("LevelData")]
    public interface ILevelData : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets name of the current level.
        /// </summary>
        string LevelName { get; set; }

        /// <summary>
        /// Gets or sets name of the level it unlocks.
        /// </summary>
        string UnlocksLevel { get; set; }

        /// <summary>
        /// Gets or sets number of secret found.
        /// </summary>
        int SecretsFound { get; set; }

        /// <summary>
        /// Gets or sets number of total secrets.
        /// </summary>
        int TotalNumberOfSecrets { get; set; }
    }
}
