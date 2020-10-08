using System;
using Xunit;
using Pashua.BooleanExtensions;

namespace Pashua.Tests {

    public class BooleanExtensionsTests {
	
		[Theory]
        [InlineData(true,1)]
		[InlineData(false,0)]
		public void ShouldConvertBooleanValues( bool value, int expected )
		{
			Assert.That( value.ToInt(), Is.EqualTo( expected ), "Did not convert value." );
		}

		[Theory]
		[InlineData(true,1)]
		[InlineData(false,0)]
		[InlineData(null,0)]
		public void ShouldConvertNullableBooleanValues( bool? value, int expected )
		{
			Assert.That( value.ToInt(), Is.EqualTo( expected ), "Did not convert value." );
		}
	}
}

