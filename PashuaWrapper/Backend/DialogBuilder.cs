using System;
using System.Diagnostics;
using System.Globalization;
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

		private sealed class ControlContext {
			private static int _count;
			private readonly List<string> _script;
			private readonly string _id;

			public ControlContext( List<string> script, string id, string type ) {
				_script = script;
				_id = id ?? GetNextId();
				_script.AddFormat( "{0}.type = {1}", _id, type );
			}

			private string GetNextId() {
				return "unnamedControl" + _count++;
			}

			public void Set( string propertyName, string propertyValue ) {
				if( !String.IsNullOrWhiteSpace( propertyValue ) ) {
					_script.AddFormat( "{0}.{1} = {2}", _id, propertyName, propertyValue );
				}
			}

			public void Set( string propertyName, bool? propertyValue, bool defaultValue ) {
				if( propertyValue.HasValue && propertyValue != defaultValue ) {
					_script.AddFormat( "{0}.{1} = 1", _id, propertyName );
				}
			}

			public void Set( string propertyName, int? propertyValue ) {
				if( propertyValue.HasValue ) {
					_script.AddFormat( "{0}.{1} = {2}", _id, propertyName, propertyValue.Value );
				}
			}
		}

		private string GetRandomId() {
			return new String( 
				Enumerable.Range( 0, count: 20 ).
                Select( _ => (char)_random.Next( 'a', 'z' ) ).
                ToArray() );
		}

		private ControlContext CreateControl( string type, string id = null ) {
			return new ControlContext( _script, id: id, type: type );
		}

		public DialogBuilder WithDefaultButton( string id = null, 
		                                        string label = null, 
		                                        bool? enabled = null, 
		                                        string tooltip = null ) {
			var control = CreateControl( type: "defaultbutton", id: id );
			control.Set( "label", label );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			return this;
		}

		public DialogBuilder WithButton( string id,
		                                 string label, 
		                                 int? x = null,
		                                 int? y = null,
		                                 bool? enabled = null, 
		                                 string tooltip = null ) {
			var control = CreateControl( type: "button", id: id );
			control.Set( "label", label );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			return this;
		}

		public DialogBuilder WithCancelButton( string id = "cancel",
		                                       string label = null, 
		                                       bool? enabled = null, 
		                                       string tooltip = null ) {
			var control = CreateControl( type: "cancelbutton", id: id );
			control.Set( "label", label );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			return this;
		}

		public DialogBuilder WithCheckBox( string id,
		                                   string label,
		                                   bool? enabled = null,
		                                   string tooltip = null,
		                                   int? x = null,
		                                   int? y = null,
		                                   int? relativeX = null,
		                                   int? relativeY = null ) {
			var control = CreateControl( type: "checkbox", id: id );
			control.Set( "label", label );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relX", relativeX );
			control.Set( "relY", relativeY );
			return this;
		}

		public DialogBuilder WithComboBox( string id,
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
			var control = CreateControl( type: "combobox", id: id );
			control.Set( "label", label );
			foreach( var option in options ) {
				control.Set( "option", option );
			}
			control.Set( "default", defaultOption );
			if( completion != AutoCompletion.CaseSensitive ) {
				control.Set( "completion", ((int)completion).ToString( CultureInfo.InvariantCulture ) );
			}
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "width", width );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relX", relativeX );
			control.Set( "relY", relativeY );
			return this;
		}

		public DialogBuilder WithDate( string id,
		                               bool textual = false,
		                               bool chooseDate = true,
		                               bool chooseTime = false,
		                               DateTime? defaultDateTime = null,
		                               int? x = null,
		                               int? y = null,
		                               bool? enabled = false,
		                               string tooltip = null ) {
			var control = CreateControl( type: "date", id: id );
			control.Set( "textual", textual, defaultValue: false );
			control.Set( "date", chooseDate, defaultValue: true );
			control.Set( "time", chooseTime, defaultValue: false );
			if( defaultDateTime.HasValue ) {
				control.Set( "default", defaultDateTime.Value.ToString( "yyyy-MM-dd HH:mm" ) );
			}
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
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

