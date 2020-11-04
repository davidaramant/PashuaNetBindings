using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class CheckBoxTests
    {
        [Fact]
        public void ShouldValidateRelYProperty()
        {
            var checkBox = new CheckBox
            {
                Label = "Text",
                RelY = -20,
            };

            checkBox.GetValidationIssues().Should().HaveCount(1);
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("1", true)]
        public void ShouldCallCheckedBasedOnResult(string rawResult, bool expectedResult)
        {
            bool calledChecked = false;
            var button = new CheckBox { Label = "button", Checked = () => calledChecked = true, };
            ((IHaveResults)button).SetResult(rawResult);
            calledChecked.Should().Be(expectedResult);
        }
    }
}
