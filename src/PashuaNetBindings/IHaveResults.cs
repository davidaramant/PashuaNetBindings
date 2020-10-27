using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PashuaNetBindings.Tests")]

namespace Pashua
{
    /// <summary>
    /// Internal interface used to set results on the element.
    /// </summary>
    internal interface IHaveResults
    {
        /// <summary>
        /// Intentionally shadows <see cref="IPashuaControl.Id"/>
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Sets the raw result on the control.
        /// </summary>
        /// <param name="result">The raw result.</param>
        void SetResult(string result);
    }
}
