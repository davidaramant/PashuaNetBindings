using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class CancelButtonTests
    {
        [Fact]
        public void ShouldNotRequireLabelToBeSet()
        {
            var cancelButton = new CancelButton();
            cancelButton.GetValidationIssues().Should().BeEmpty();
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("1", true)]
        public void ShouldCallClickedBasedOnResult(string rawResult, bool expectedResult)
        {
            bool calledClicked = false;
            var button = new CancelButton() { Label = "button", Clicked = () => calledClicked = true, };
            ((IHaveResults)button).SetResult(rawResult);
            calledClicked.Should().Be(expectedResult);
        }
    }
}
