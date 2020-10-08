using System;
using System.Collections.Generic;
using Xunit;
using Pashua.ListExtensions;

namespace Pashua.Tests {
	public class ListExtensionsTests {
		[Fact]
		public void ShouldAddFormatedStringToList()
		{
			const string format = "Format: {0}";
			const string args = "100";

			var list = new List<string>();
			list.AddFormat( format, args );

			Assert.That( list, Has.Count.EqualTo( 1 ), "Added incorrect number of entries." );
			Assert.That( list[ 0 ], Is.EqualTo( String.Format( format, args ) ),
			             "Did not added formatted string." );
		}
	}
}

