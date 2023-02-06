using System;
using System.Collections.Generic;

namespace Implementation.Data
{
    [Serializable]
    public class LevelData : ILevelData
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string LevelName { get; set; }

        /// <inheritdoc/>
        public string UnlocksLevel { get; set; }

        /// <inheritdoc/>
        public int SecretsFound { get; set; }

        /// <inheritdoc/>
        public int TotalNumberOfSecrets { get; set; }
    }
}
