using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            var elementLookup = script.ToDictionary(c => c.Id, c=>c);

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

            foreach (var control in script)
            {
                control.WriteTo(process.StandardInput);
            }

            process.StandardInput.Close();
            process.WaitForExit();

            var results = ParseResults(process.StandardOutput);

            // TODO: Use elementLookup together with results
        }

        private static string GetPashuaPath(string customPashuaPath) => 
            customPashuaPath ??
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Pashua.app",
                    "Contents",
                    "MacOS",
                    "Pashua");

        private static Dictionary<string, string> ParseResults(StreamReader reader)
        {
            var results = new Dictionary<string, string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split('=');
                    results.Add(parts[0], parts[1]);
                }
            }

            return results;
        }
    }
}