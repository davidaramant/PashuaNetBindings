using System.Collections.Generic;

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
    }
}
