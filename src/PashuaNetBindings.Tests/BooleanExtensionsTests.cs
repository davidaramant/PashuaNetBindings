using FluentAssertions;
using Pashua.BooleanExtensions;
using Xunit;

namespace Pashua.Tests
{
    public class BooleanExtensionsTests {
	
		[Theory]
        [InlineData(true,1)]
		[InlineData(false,0)]
		public void ShouldConvertBooleanValues( bool value, int expected )
        {
            value.ToInt().Should().Be(expected);
		}

		[Theory]
		[InlineData(true,1)]
		[InlineData(false,0)]
		[InlineData(null,0)]
		public void ShouldConvertNullableBooleanValues( bool? value, int expected )
		{
            value.ToInt().Should().Be(expected);
		}
	}
}

