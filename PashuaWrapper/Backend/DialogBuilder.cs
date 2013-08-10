using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using PashuaWrapper.ListExtensions;
using PashuaWrapper.BooleanExtensions;

namespace PashuaWrapper.Backend {
	public sealed class DialogBuilder {
		private readonly Random _random = new Random();
		private readonly List<string> _script = new List<string>();

		internal DialogBuilder( IEnumerable<string> script ) {
			_script.AddRange( script );
		}

		private string GetRandomId() {
			return new String( 
				Enumerable.Range( 0, count: 20 ).
                Select( _ => (char)_random.Next( 'a', 'z') ).
                ToArray() );
		}

		public DialogBuilder WithDefaultButton( 
								string id = null, 
								string label = null, 
								bool? enabled = null, 
								string tooltip = null ) {
			id = id ?? GetRandomId();

			_script.AddFormat( "{0}.type = defaultbutton", id );
			if( !String.IsNullOrWhiteSpace( label ) ) {
				_script.AddFormat( "{0}.label = {1}", id, label );
			}
			if( enabled.HasValue && !enabled.Value ) {
				_script.AddFormat( "{0}.disabled = 1", id );
			}
			if( !String.IsNullOrWhiteSpace( tooltip ) ) {
				_script.AddFormat( "{0}.tooltip = {1}", id, tooltip );
			}

			return this;
		}		

		public DialogBuilder WithButton( 
								string id,
                                string label, 
		                        int? x = null,
		                        int? y = null,
								bool? enabled = null, 
								string tooltip = null ) {
			_script.AddFormat( "{0}.type = button", id );
			_script.AddFormat( "{0}.label = {1}", id, label );
			if( x.HasValue ) {
				_script.AddFormat( "{0}.x = {1}", id, x );
			}
			if( y.HasValue ) {
				_script.AddFormat( "{0}.y = {1}", id, y );
			}
			if( enabled.HasValue && !enabled.Value ) {
				_script.AddFormat( "{0}.disabled = 1", id );
			}
			if( !String.IsNullOrWhiteSpace( tooltip ) ) {
				_script.AddFormat( "{0}.tooltip = {1}", id, tooltip );
			}

			return this;
		}

		public DialogBuilder WithCancelButton( 
		                                string id = "cancel",
		                                string label = null, 
		                                bool? enabled = null, 
		                                string tooltip = null ) {
			_script.AddFormat( "{0}.type = cancelbutton", id );
			_script.AddFormat( "{0}.label = {1}", id, label );
			if( enabled.HasValue && !enabled.Value ) {
				_script.AddFormat( "{0}.disabled = 1", id );
			}
			if( !String.IsNullOrWhiteSpace( tooltip ) ) {
				_script.AddFormat( "{0}.tooltip = {1}", id, tooltip );
			}

			return this;
		}

		public DialogBuilder WithCheckBox(
			string id,
			string label,
			bool? enabled = null,
			string tooltip = null,
			int? x = null,
			int? y = null,
			int? relativeX = null,
			int? relativeY = null ) {
			_script.AddFormat( "{0}.type = checkbox", id );
			_script.AddFormat( "{0}.label = {1}", id, label );
			if( enabled.HasValue && !enabled.Value ) {
				_script.AddFormat( "{0}.disabled = 1", id );
			}
			if( !String.IsNullOrWhiteSpace( tooltip ) ) {
				_script.AddFormat( "{0}.tooltip = {1}", id, tooltip );
			}
			if( x.HasValue ) {
				_script.AddFormat( "{0}.x = {1}", id, x );
			}
			if( y.HasValue ) {
				_script.AddFormat( "{0}.y = {1}", id, y );
			}
			if( relativeX.HasValue ) {
				_script.AddFormat( "{0}.relx = {1}", id, relativeX );
			}
			if( relativeY.HasValue ) {
				_script.AddFormat( "{0}.rely = {1}", id, relativeY );
			}

			return this;
		}		

		public DialogBuilder WithComboBox(
			string id,
			string label,
			IEnumerable<string> options,
			string defaultOption = null,
			AutoCompletion completion = AutoCompletion.CaseSensitive,
			bool? enabled = null,
			string tooltip = null,
			int? width = null,	
			int? x = null,
			int? y = null,
			int? relativeX = null,
			int? relativeY = null ) {
			_script.AddFormat( "{0}.type = combobox", id );
			_script.AddFormat( "{0}.label = {1}", id, label );

			foreach( var option in options ) {
				_script.AddFormat( "{0}.option = {1}", id, option );
			}

			if( !String.IsNullOrWhiteSpace( defaultOption ) ) {
				_script.AddFormat( "{0}.default = {1}", id, defaultOption );	
			}

			if( completion != AutoCompletion.CaseSensitive ) {
				_script.AddFormat( "{0}.completion = {1}", id, (int)completion );
			}
			if( enabled.HasValue && !enabled.Value ) {
				_script.AddFormat( "{0}.disabled = 1", id );
			}
			if( !String.IsNullOrWhiteSpace( tooltip ) ) {
				_script.AddFormat( "{0}.tooltip = {1}", id, tooltip );
			}
			if( width.HasValue ) {
				_script.AddFormat( "{0}.width = {1}", id, width );
			}
			if( x.HasValue ) {
				_script.AddFormat( "{0}.x = {1}", id, x );
			}
			if( y.HasValue ) {
				_script.AddFormat( "{0}.y = {1}", id, y );
			}
			if( relativeX.HasValue ) {
				_script.AddFormat( "{0}.relx = {1}", id, relativeX );
			}
			if( relativeY.HasValue ) {
				_script.AddFormat( "{0}.rely = {1}", id, relativeY );
			}

			return this;
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

