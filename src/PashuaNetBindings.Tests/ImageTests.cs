using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class ImageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldValidatePath(string invalidValue)
        {
            var image = new Image
            {
                Path = invalidValue,
            };
            image.GetValidationIssues().Should().HaveCount(1);
        }
    }
}
