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
        public void ShouldSetWasClickedBasedOnResult(string rawResult, bool expectedResult)
        {
            var button = new CancelButton();
            ((IHaveResults)button).SetResult(rawResult);
            button.WasClicked.Should().Be(expectedResult);
        }
    }
}
