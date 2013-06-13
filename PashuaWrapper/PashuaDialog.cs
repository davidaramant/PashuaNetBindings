using System;
using System.Collections.Generic;
using PashuaWrapper.Backend;
using PashuaWrapper.ListExtensions;
using PashuaWrapper.BooleanExtensions;

namespace PashuaWrapper {
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

			if( !String.IsNullOrWhiteSpace( title ) ) {
				script.AddFormat( "*.title = {0}", title );
			}

			if( transparency.HasValue ){
				script.AddFormat( "*.transparency = {0}", transparency );
			}

			if( brushedMetal ) {
				script.Add( "*.appearance = metal" );
			}

			if( autoCloseTimeSeconds.HasValue ) {
				script.AddFormat( "*.autoclosetime = {0}", autoCloseTimeSeconds );
			}

			if( floating.HasValue ) {
				script.AddFormat( "*.floating = {0}", floating.ToInt() );
			}

			if( x.HasValue ) {
				script.AddFormat( "*.x = {0}", x );
			}

			if( y.HasValue ) {
				script.AddFormat( "*.y = {0}", y );
			}

			return new DialogBuilder( script );
		}
	}
}

