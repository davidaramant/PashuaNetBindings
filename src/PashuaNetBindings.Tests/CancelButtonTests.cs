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
    }
}
