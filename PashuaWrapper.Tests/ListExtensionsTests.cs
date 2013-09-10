using System;
using System.Collections.Generic;
using NUnit.Framework;
using PashuaWrapper.ListExtensions;

namespace PashuaWrapper.Tests {
	[TestFixture]
	public class ListExtensionsTests {
		[Test]
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

