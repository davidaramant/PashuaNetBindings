using System;
using System.Collections.Generic;
using PashuaWrapper.Backend;
using PashuaWrapper.ListExtensions;
using PashuaWrapper.BooleanExtensions;

namespace PashuaWrapper {
	public enum AutoCompletion {
		None = 0,
		CaseSensitive = 1,
		CaseInsensitive = 2,
	}

	public enum FontSize {
		Regular,
		Small,
		Mini
	}

	public static class PashuaDialog {
		public static DialogBuilder Create( 
				string title = null,
				double? transparency = null,
				bool brushedMetal = false,
				int? autoCloseTimeSeconds = null,
				bool? floating = false,
				int? x = null,
				int? y = null ) {
			var script = new List<string>();
			var context = new ControlContext( script );
			context.Set( "title", title );
			context.Set( "transparency", transparency );
			context.Set( "autoclosetime", autoCloseTimeSeconds );
			context.Set( "floating", floating, defaultValue: false );
			context.Set( "x", x );
			context.Set( "y", y );

			if( brushedMetal ) {
				script.Add( "*.appearance = metal" );
			}

			return new DialogBuilder( script );
		}
	}
}

