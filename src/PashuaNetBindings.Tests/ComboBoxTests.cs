using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class ComboBoxTests
    {
        [Fact]
        public void ShouldRequireAtLeastOneOption()
        {
            var comboBox = new ComboBox();
            comboBox.GetValidationIssues().Should().HaveCount(1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotAllowEmptyOptions(string value)
        {
            var comboBox = new ComboBox{Options = new []{value}};
            comboBox.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldNotReportErrorsWithAtLeastOneOption()
        {
            var comboBox = new ComboBox{Options = new []{"value"}};
            comboBox.GetValidationIssues().Should().BeEmpty();
        }

        [Fact]
        public void ShouldRequirePositiveRows()
        {
            var comboBox = new ComboBox
            {
                Options = new []{"value"}, 
                Rows = -1,
            };
            comboBox.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldRequirePositiveWidth()
        {
            var comboBox = new ComboBox
            {
                Options = new []{"value"}, 
                Width = -1,
            };
            comboBox.GetValidationIssues().Should().HaveCount(1);
        }
    }
}
