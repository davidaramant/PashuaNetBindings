using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace PashuaWrapper.Backend {
	public sealed class DialogBuilder {
		private readonly List<string> _script = new List<string>();

		internal DialogBuilder( IEnumerable<string> script ) {
			_script.AddRange( script );
		}

		public void Show() {
			var startInfo = new ProcessStartInfo {
				UseShellExecute = false,
				CreateNoWindow = true,
				FileName = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ), "Pashua.app", "Contents" ,"MacOS","Pashua"),
				Arguments = "-",
				RedirectStandardInput = true,
			};

			using( var process = Process.Start( startInfo ) ) {
				foreach( var line in _script ) {
					process.StandardInput.WriteLine( line );
				}
				process.StandardInput.Close();
				process.WaitForExit();
			}
		}
	}
}

