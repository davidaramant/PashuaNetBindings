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
            var button = new Button { Label = label };
            button.GetValidationIssues().Should().HaveCount(1);
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("1", true)]
        public void ShouldSetWasClickedBasedOnResult(string rawResult, bool expectedResult)
        {
            var button = new Button { Label = "button" };
            ((IHaveResults)button).SetResult(rawResult);
            button.WasClicked.Should().Be(expectedResult);
        }
    }
}
