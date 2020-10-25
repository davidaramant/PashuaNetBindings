using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Pashua
{
    public static class ScriptRunner
    {
        public static void Run(IEnumerable<IPashuaControl> script)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pashua.app",
                    "Contents", "MacOS", "Pashua"),
                Arguments = "-",
                RedirectStandardInput = true
            };

            using (var process = Process.Start(startInfo))
            {
                //foreach (var line in _script) process.StandardInput.WriteLine(line);
                process.StandardInput.Close();
                process.WaitForExit();
            }
        }
    }
}