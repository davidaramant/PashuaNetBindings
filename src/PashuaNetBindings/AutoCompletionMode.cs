namespace Pashua
{
    /// <summary>
    /// Modes of auto-completion.
    /// </summary>
    public enum AutoCompletionMode
    {
        /// <summary>
        /// No completion.
        /// </summary>
        None = 0,

        /// <summary>
        /// Completes the first item in the completion list which matches the entered string, case-sensitive
        /// </summary>
        CaseSensitive = 1,
        
        /// <summary>
        /// Completes the first item in the completion list which matches the entered string, case-insensitive
        /// </summary>
        CaseInsensitive = 2
    }
}