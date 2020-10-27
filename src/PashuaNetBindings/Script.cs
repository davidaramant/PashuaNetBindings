using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pashua
{
    /// <summary>
    /// Used to execute a Pashua script.
    /// </summary>
    public static class Script
    {
        /// <summary>
        /// Runs the specified script.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        /// <param name="customPashuaPath">(Optional) A custom path to the Pashua executable, if it's not installed in your Applications folder.</param>
        /// <exception cref="PashuaScriptException">Thrown if there was an error in the script.</exception>
        public static void Run(IEnumerable<IPashuaControl> script, string customPashuaPath = null)
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
    }
}