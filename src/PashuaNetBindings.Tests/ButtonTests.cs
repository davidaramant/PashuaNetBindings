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
        public void ShouldCallClickedBasedOnResult(string rawResult, bool expectedResult)
        {
            bool calledClicked = false;
            var button = new Button { Label = "button", Clicked = () => calledClicked = true, };
            ((IHaveResults)button).SetResult(rawResult);
            calledClicked.Should().Be(expectedResult);
        }
    }
}
