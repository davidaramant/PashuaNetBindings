using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class TextBoxTests
    {
        [Fact]
        public void ShouldReplaceReturnTokenWithNewline()
        {
            string result = null;

            var textBox = new TextBox { TextEntered = t => result = t, };
            ((IHaveResults)textBox).SetResult("One[return]Two[return]Three");
            result.Should().Be("One\nTwo\nThree");
        }
    }
}
