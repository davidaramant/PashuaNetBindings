using System.Collections.Generic;
using System.IO;

namespace Pashua
{
    /// <summary>
    /// A Pashua control.
    /// </summary>
    public interface IPashuaControl
    {
        /// <summary>
        /// Serializes the control to the given writer.
        /// </summary>
        /// <param name="writer">The writer to serialize to.</param>
        /// <exception cref="PashuaScriptException">Thrown if the control was not configured correctly.</exception>
        void WriteTo(StreamWriter writer);

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        IEnumerable<string> GetValidationIssues();
    }
}
