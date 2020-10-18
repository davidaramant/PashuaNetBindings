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
        /// <exception cref="PashuaControlSetupException">Thrown if the control was not configured correctly.</exception>
        void WriteTo(StreamWriter writer);
    }
}
