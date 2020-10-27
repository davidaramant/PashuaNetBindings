using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class TextTests
    {
        [Fact]
        public void ShouldValidateThatDefaultIsSet()
        {
            var text = new Text();
            text.GetValidationIssues().Should().HaveCount(1);
        }
    }
}
