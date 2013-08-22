using System;
using PashuaWrapper;

namespace Runner {
	class MainClass {
		public static void Main( string[] args ) {
			PashuaDialog.
				Create( title: "Hello" ).
				WithButton( id: "one", label: "Another option" ).
				WithDefaultButton( label: "Yup" ).
				WithCancelButton( id: "cancel", label: "Cancel button" ).
				WithCheckBox( id: "check", label: "Checkbox", tooltip: "Yup" ).
				WithComboBox( id: "combo",
			               label: "Here are some options",
			               options: new[] { "Cat", "Dog", "Horse" },
			               defaultOption: "Dog",
			               completion: AutoCompletion.CaseInsensitive ).
				WithDate( id: "Date1", chooseTime: true, defaultDateTime: new DateTime( 2013, 01, 01, 01, 02, 03 ) ).
				WithImage( path: "test.png", label: "Test image", border: true, tooltip: "Sure is" ).
				WithOpenBrowser( id: "browser",
			                  label: "Select an image",
			                  defaultPath: "test.png",
			                  fileTypes: new[] { "jpg", "gif", "png" },
			                  width: 350 ).
				WithPassword( id: "password", label: "Enter a password", defaultText: "secret", width: 120 ).
				WithPopup( id: "popup", label: "Pick a number:", options: new[] { "One", "Two", "Three" }, defaultValue: "Two" ).
				WithRadioButtons( id: "radiobuttons",
			                   label: "Pick a number:",
			                   options: new[] { "One", "Two", "Three" },
			                   defaultValue: "Two" ).
				WithSaveBrowser( id: "save", label: "Set save path:", fileType: "png" ).
				WithText( text: "Here is some text" ).
				WithTextBox( id: "box", defaultText: "First line\nSecond line", monospaceFont: true ).
				WithTextField( id: "textField", label: "Enter a string", defaultText: "secret", width: 120 ).
				Show();
		}
	}
}
