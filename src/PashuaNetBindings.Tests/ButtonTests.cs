using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class ButtonTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldValidateLabel(string label)
        {
            var button = new Button {Label = label};
            button.GetValidationIssues().Should().HaveCount(1);
        }
    }
}
