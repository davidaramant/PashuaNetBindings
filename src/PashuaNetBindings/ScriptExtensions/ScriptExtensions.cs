using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Pashua.ScriptExtensions
{
    /// <summary>
    /// Create and run scripts.
    /// </summary>
    public static class ScriptExtensions
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
        /// <param name="script">The script to execute.</param>
        /// <param name="customPashuaPath">(Optional) A custom path to the Pashua executable, if it's not installed in your Applications folder.</param>
        /// <exception cref="PashuaScriptException">Thrown if there was an error in the script.</exception>
        public static void Run(this IEnumerable<IPashuaControl> script, string customPashuaPath = null)
        {
            // Enumerate the script to avoid any weird issues with lazy sequences
            var scriptCopy = script.ToArray();

            var elementWithResultLookup = 
                scriptCopy
                    .OfType<IHaveResults>()
                    .ToDictionary(c => c.Id, c=>c);

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = GetPashuaPath(customPashuaPath),
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

            foreach(var (id,value) in GetResultValues(process.StandardOutput))
            {
                elementWithResultLookup[id].SetResult(value);
            }
        }

        private static string GetPashuaPath(string customPashuaPath) => 
            customPashuaPath ??
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Pashua.app",
                    "Contents",
                    "MacOS",
                    "Pashua");

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
