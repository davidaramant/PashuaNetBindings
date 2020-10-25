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
    }
}
