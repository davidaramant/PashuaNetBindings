using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class TextBoxTests
    {
        [Fact]
        public void ShouldReplaceReturnTokenWithNewline()
        {
            var textBox = new TextBox();
            ((IHaveResults)textBox).SetResult("One[return]Two[return]Three");
            textBox.EnteredText.Should().Be("One\nTwo\nThree");
        }
    }
}
