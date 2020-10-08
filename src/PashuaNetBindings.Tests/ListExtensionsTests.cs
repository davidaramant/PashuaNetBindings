using FluentAssertions;
using Pashua.ListExtensions;
using System.Collections.Generic;
using Xunit;

namespace Pashua.Tests
{
    public class ListExtensionsTests {
		[Fact]
		public void ShouldAddFormattedStringToList()
		{
			const string format = "Format: {0}";
			const string args = "100";

			var list = new List<string>();
			list.AddFormat( format, args );

            list.Should().HaveCount(1);
            list[0].Should().Be(string.Format(format, args));
        }
	}
}

