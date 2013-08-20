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
				WithDate( id: "Date1", chooseTime:true, defaultDateTime: new DateTime( 2013, 01, 01, 01, 02, 03 ) ).
				Show();
		}
	}
}
