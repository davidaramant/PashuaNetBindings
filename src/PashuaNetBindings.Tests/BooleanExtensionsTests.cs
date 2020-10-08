using System;
using NUnit.Framework;
using PashuaWrapper.BooleanExtensions;

namespace PashuaWrapper.Tests {
	[TestFixture]
	public class BooleanExtensionsTests {
		[TestCase(true,1)]
		[TestCase(false,0)]
		public void ShouldConvertBooleanValues( bool value, int expected )
		{
			Assert.That( value.ToInt(), Is.EqualTo( expected ), "Did not convert value." );
		}

		[TestCase(true,1)]
		[TestCase(false,0)]
		[TestCase(null,0)]
		public void ShouldConvertNullableBooleanValues( bool? value, int expected )
		{
			Assert.That( value.ToInt(), Is.EqualTo( expected ), "Did not convert value." );
		}
	}
}

