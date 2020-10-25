using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class OpenBrowserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyStringInFileTypes(string invalidValue)
        {
            var openBrowser = new OpenBrowser
            {
                FileTypes = new []{invalidValue},
            };
            openBrowser.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldAllowNotSpecifyingFileTypes()
        {
            var openBrowser = new OpenBrowser();
            openBrowser.GetValidationIssues().Should().BeEmpty();
        }
    }
}
