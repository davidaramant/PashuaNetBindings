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

		private string GetRandomId() {
			return new String( 
				Enumerable.Range( 0, count: 20 ).
                Select( _ => (char)_random.Next( 'a', 'z' ) ).
                ToArray() );
		}

		private ControlContext CreateControl( string type, string id = null ) {
			return new ControlContext( _script, id: id, type: type );
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

		public DialogBuilder WithImage( string path,
		                                string id = null,
		                                string label = null, 
		                                bool border = false,
		                                int? maxWidth = null,
		                                int? maxHeight = null,
		                                string tooltip = null,
		                                int? x = null,
		                                int? y = null,
		                                int? relativeX = null,
		                                int? relativeY = null ) {
			var control = CreateControl( type: "image", id: id );
			control.Set( "path", path );
			control.Set( "label", label );
			control.Set( "border", border, defaultValue: false );
			control.Set( "maxwidth", maxWidth );
			control.Set( "maxheight", maxHeight );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithOpenBrowser( string id,
		                                      string label = null,
		                                      int? width = null,
		                                      string defaultPath = null,
		                                      IEnumerable<string> fileTypes = null,
		                                      int? x = null,
		                                      int? y = null,
		                                      int? relativeX = null,
		                                      int? relativeY = null ) {
			var control = CreateControl( type: "openbrowser", id: id );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "default", defaultPath );
			if( fileTypes != null ) {
				control.Set( "filetype", String.Join( " ", fileTypes ) );
			}
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithPassword( string id,
		                                   string label = null,
		                                   int? width = null,
		                                   string defaultText = null,
		                                   bool enabled = true,
		                                   string tooltip = null,
		                                   int? x = null,
		                                   int? y = null,
		                                   int? relativeX = null,
		                                   int? relativeY = null ) {
			var control = CreateControl( type: "password", id: id );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "default", defaultText );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithPopup( string id,
		                                IEnumerable<string> options,
		                                string defaultValue = null,
		                                string label = null,
		                                bool enabled = true,
		                                string tooltip = null,
		                                int? width = null,
		                                int? x = null,
		                                int? y = null,
		                                int? relativeX = null,
		                                int? relativeY = null ) {
			var control = CreateControl( type: "popup", id: id );
			control.Set( "label", label );
			foreach( var option in options ) {
				control.Set( "option", option );
			}
			control.Set( "default", defaultValue );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "width", width );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithRadioButtons( string id,
		                                       IEnumerable<string> options,
		                                       string defaultValue = null,
		                                       string label = null,
		                                       bool enabled = true,
		                                       string tooltip = null,
		                                       int? x = null,
		                                       int? y = null,
		                                       int? relativeX = null,
		                                       int? relativeY = null ) {
			var control = CreateControl( type: "radiobutton", id: id );
			control.Set( "label", label );
			foreach( var option in options ) {
				control.Set( "option", option );
			}
			control.Set( "default", defaultValue );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithSaveBrowser( string id,
		                                      string label = null,
		                                      int? width = null,
		                                      string defaultPath = null,
		                                      string fileType = null,
		                                      int? x = null,
		                                      int? y = null,
		                                      int? relativeX = null,
		                                      int? relativeY = null ) {
			var control = CreateControl( type: "savebrowser", id: id );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "default", defaultPath );
			control.Set( "tooltip", fileType );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithText( string text,
		                               string id = null,
		                               string label = null,
		                               int? width = null,
		                               string tooltip = null,
		                               int? x = null,
		                               int? y = null,
		                               int? relativeX = null,
		                               int? relativeY = null ) {
			var control = CreateControl( type: "text", id: id );
			control.Set( "text", EscapeNewLines( text ) );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithTextBox( string id,
		                                  string label = null,
		                                  int? width = null,
		                                  int? height = null,
		                                  string defaultText = null,
		                                  FontSize fontSize = FontSize.Regular,
		                                  bool monospaceFont = false,
		                                  bool enabled = true,
		                                  string tooltip = null,
		                                  int? x = null,
		                                  int? y = null,
		                                  int? relativeX = null,
		                                  int? relativeY = null ) {
			var control = CreateControl( type: "textbox", id: id );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "height", height );
			control.Set( "default", EscapeNewLines( defaultText ) );
			switch( fontSize ) {
				case FontSize.Mini:
					control.Set( "fontsize", "mini" );
				case FontSize.Small:
					control.Set( "fontsize", "small" );
				case FontSize.Regular:
				default:
					// Do nothing
					break;
			}
			if( monospaceFont ) {
				control.Set( "fonttype", "fixed" );
			}
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		public DialogBuilder WithTextField( string id,
		                                    string label = null,
		                                    int? width = null,
		                                    string defaultText = null,
		                                    bool enabled = true,
		                                    string tooltip = null,
		                                    int? x = null,
		                                    int? y = null,
		                                    int? relativeX = null,
		                                    int? relativeY = null ) {
			var control = CreateControl( type: "textfield", id: id );
			control.Set( "label", label );
			control.Set( "width", width );
			control.Set( "default", defaultText );
			control.Set( "disabled", !enabled, defaultValue: false );
			control.Set( "tooltip", tooltip );
			control.Set( "x", x );
			control.Set( "y", y );
			control.Set( "relx", relativeX );
			control.Set( "rely", relativeY );
			return this;
		}

		private static string EscapeNewLines( string text ) {
			if( text == null ) {
				return null;
			}
			return text.Replace( "\n", "[return]" );
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

