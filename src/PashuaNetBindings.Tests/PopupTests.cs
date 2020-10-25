using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class PopupTests
    {
        [Fact]
        public void ShouldRequireThatDefaultMatchAnEntryInOptions()
        {
            var popup = new Popup
            {
                Options = new[]{"A","B"},
                Default = "C",
            };
            popup.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldAllowDefaultThatMatchesAnEntryInOptions()
        {
            var popup = new Popup
            {
                Options = new[]{"A","B"},
                Default = "A",
            };
            popup.GetValidationIssues().Should().BeEmpty();
        }

        [Fact]
        public void ShouldAllowNoDefault()
        {
            var popup = new Popup
            {
                Options = new[]{"A","B"},
            };
            popup.GetValidationIssues().Should().BeEmpty();
        }
    }
}
