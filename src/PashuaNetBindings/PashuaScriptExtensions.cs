﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// Create and run scripts.
    /// </summary>
    public static class PashuaScriptExtensions
    {
        /// <summary>
        /// Adds the given control to the script and returns it.
        /// </summary>
        /// <remarks>
        /// Useful convenience since references to controls that return output need to be kept around.
        /// </remarks>
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
        /// Runs the specified script.
        /// </summary>
        /// <remarks>
        /// If the path to the app is not specified, it will look in the following places for Pashua.app:
        /// <list type="bullet">
        /// <item>/Applications/Pashua.app</item>
        /// <item>~/Applications/Pashua.app</item>
        /// <item>./Pashua.app</item>
        /// </list>
        /// </remarks>
        /// <param name="script">The script to execute.</param>
        /// <param name="pashuaAppPath">(Optional) A custom path to Pashua.app.</param>
        /// <exception cref="PashuaScriptException">Thrown if there was an error in the script.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the Pashua executable could not be located.</exception>
        public static void RunScript(this IEnumerable<IPashuaControl> script, string pashuaAppPath = null)
        {
            // Enumerate the script to avoid any weird issues with lazy sequences
            var scriptCopy = script.ToArray();

            var elementWithResultLookup =
                scriptCopy
                    .OfType<IHaveResults>()
                    .ToDictionary(c => c.Id, c => c);

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = PashuaAppFinder.GetExecutablePath(pashuaAppPath),
                Arguments = "-",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            };

            using var process = Process.Start(startInfo);

            foreach (var control in scriptCopy)
            {
                control.WriteTo(process.StandardInput);
            }

            process.StandardInput.Close();
            process.WaitForExit();

            foreach (var (id, value) in GetResultValues(process.StandardOutput))
            {
                elementWithResultLookup[id].SetResult(value);
            }
        }

        private static IEnumerable<(string id, string value)> GetResultValues(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split('=');
                    yield return (parts[0], parts[1]);
                }
            }
        }

        /// <summary>
        /// Writes the script to the given writer.
        /// </summary>
        /// <remarks>
        /// Mostly useful for debugging, or if you want to migrate to static Pashua script files.
        /// </remarks>
        /// <param name="script">The script.</param>
        /// <param name="writer">The output writer.</param>
        public static void WriteTo(this IEnumerable<IPashuaControl> script, TextWriter writer)
        {
            foreach (var control in script)
            {
                control.WriteTo(writer);
            }
        }
    }
}