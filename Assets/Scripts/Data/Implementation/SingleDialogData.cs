using System;

namespace Implementation.Data
{
    [Serializable]
    public class SingleDialogData : ISingleDialogData
    {
        /// <inheritdoc />
        public string CharacterIcon { get; set; }

        /// <inheritdoc />
        public string CharacterName { get; set; }

        /// <inheritdoc />
        public string Text { get; set; }
    }
}
