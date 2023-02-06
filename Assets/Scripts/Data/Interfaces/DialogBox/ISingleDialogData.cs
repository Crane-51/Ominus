namespace Implementation.Data
{
    public interface ISingleDialogData
    {
        /// <summary>
        /// Gets or sets resource path of character icon.
        /// </summary>
        string CharacterIcon { get; set; }

        /// <summary>
        /// Gets or sets character name.
        /// </summary>
        string CharacterName { get; set; }

        /// <summary>
        /// Gets or sets text of dialog.
        /// </summary>
        string Text { get; set; }
    }
}
