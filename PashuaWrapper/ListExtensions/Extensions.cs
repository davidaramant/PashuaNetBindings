using System;
using System.Collections.Generic;

namespace PashuaWrapper.ListExtensions {
	public static class Extensions {
		public static void AddFormat( this List<string> builder, string format, params object[] args ){
			builder.Add( String.Format( format, args ) );
		}
	}
}

