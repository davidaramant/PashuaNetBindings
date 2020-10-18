using System;

namespace Pashua
{
    /// <summary>
    /// Indicates an error in setting up one of the controls.
    /// </summary>
    public sealed class PashuaControlSetupException : Exception
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="message">The message.</param>
        public PashuaControlSetupException(string message) : base(message)
        {
        }
    }
}
