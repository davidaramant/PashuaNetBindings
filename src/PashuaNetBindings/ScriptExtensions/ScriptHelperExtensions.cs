using System.Collections.Generic;
using System.IO;

namespace Pashua.ScriptExtensions
{
    public static class ScriptHelperExtensions
    {
        /// <summary>
        /// Adds the given control to the script and returns it.
        /// </summary>
        /// <typeparam name="T">The type of control being added.</typeparam>
        /// <param name="script">The script.</param>
        /// <param name="control">The control being added.</param>
        /// <returns>The control.</returns>
        public static T AddAndReturn<T>(this ICollection<IPashuaControl> script, T control) where T : IPashuaControl
        {
            script.Add(control);
            return control;
        }

        /// <summary>
        /// Writes the script to the given writer.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="writer">The output writer.</param>
        internal static void WriteTo(this IEnumerable<IPashuaControl> script, TextWriter writer)
        {
            foreach (var control in script)
            {
                control.WriteTo(writer);
            }
        }
    }
}
