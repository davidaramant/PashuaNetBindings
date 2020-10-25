using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class WindowTests
    {
        [Fact]
        public void ShouldValidateAutoCloseTime()
        {
            var window = CreateValidWindow();

            window.AutoCloseTime = new TimeSpan(-1,0,0);

            window.GetValidationIssues().Should().HaveCount(1);
        }   
        
        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void ShouldValidateTransparency(double value)
        {
            var window = CreateValidWindow();

            window.Transparency = value;

            window.GetValidationIssues().Should().HaveCount(1);
        }       

        [Fact]
        public void ShouldValidateX()
        {
            var window = CreateValidWindow();

            window.X = -1;

            window.GetValidationIssues().Should().HaveCount(1);
        }        

        [Fact]
        public void ShouldValidateY()
        {
            var window = CreateValidWindow();

            window.Y = -1;

            window.GetValidationIssues().Should().HaveCount(1);
        }

        private Window CreateValidWindow() =>
            new Window
            {
            };
    }
}
