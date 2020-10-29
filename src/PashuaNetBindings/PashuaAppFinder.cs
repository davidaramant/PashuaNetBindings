using System;
using System.IO;

namespace Pashua
{
    internal static class PashuaAppFinder
    {
        /// <summary>
        /// Attempts to find Pashua.app
        /// </summary>
        /// <param name="customPashuaPath">(Optional) If specified, assume this is the path to Pashua.app.</param>
        /// <remarks>The path to the executable</remarks>
        /// <exception cref="PashuaScriptException">Thrown if there was an error in the script.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the Pashua executable could not be located.</exception>
        internal static string GetExecutablePath(string customPashuaPath)
        {
            const string appName = "Pashua.app";

            string appPath = null;
            if (customPashuaPath != null)
            {
                if (!Directory.Exists(customPashuaPath))
                {
                    throw new FileNotFoundException("Custom Pashua path does not exist: " + customPashuaPath);
                }

                appPath = customPashuaPath;
            }
            else
            {
                appPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    appName);

                if (!Directory.Exists(appPath))
                {
                    appPath = Path.Combine(
                        Environment.GetEnvironmentVariable("HOME"),
                        "Applications",
                        appName);

                    if (!Directory.Exists(appPath))
                    {
                        appPath = appName;

                        if (!Directory.Exists(appPath))
                        {
                            throw new FileNotFoundException($"Could not location {appName}");
                        }
                    }
                }

            }

            var executablePath = Path.Combine(appPath, "Contents", "MacOS", "Pashua");

            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException("Could not find Pashua executable.");
            }

            return executablePath;
        }
    }
}
