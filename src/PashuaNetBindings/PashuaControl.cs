using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// A Pashua control.
    /// </summary>
    public abstract class PashuaControl
    {
        /// <summary>
        /// Serializes the control to the given writer.
        /// </summary>
        /// <param name="writer">The writer to serialize to.</param>
        /// <exception cref="PashuaScriptException">Thrown if the control was not configured correctly.</exception>
        public abstract void WriteTo(StreamWriter writer);

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        public virtual IEnumerable<string> GetValidationIssues() => Enumerable.Empty<string>();
    }
}
