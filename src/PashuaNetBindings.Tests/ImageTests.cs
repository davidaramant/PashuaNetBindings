using FluentAssertions;
using Xunit;

namespace Pashua.Tests
{
    public class ImageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldValidatePath(string invalidValue)
        {
            var image = new Image
            {
                Path = invalidValue,
            };
            image.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldValidateHeightIsPositive()
        {
            var image = new Image
            {
                Path = "path",
                Height = -1,
            };
            image.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldValidateMaxWidthIsPositive()
        {
            var image = new Image
            {
                Path = "path",
                MaxWidth = -1,
            };
            image.GetValidationIssues().Should().HaveCount(1);
        }

        [Fact]
        public void ShouldValidateMaxHeightIsPositive()
        {
            var image = new Image
            {
                Path = "path",
                MaxHeight = -1,
            };
            image.GetValidationIssues().Should().HaveCount(1);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ShouldNotAllowSettingDimensionAndMaxDimensionAtSameTime(bool setWidth, bool setMaxWidth)
        {
            var image = new Image
            {
                Path = "path",
            };

            if (setWidth)
            {
                image.Width = 1;
            }
            else
            {
                image.Height = 1;
            }

            if (setMaxWidth)
            {
                image.MaxWidth = 1;
            }
            else
            {
                image.MaxHeight = 1;
            }

            image.GetValidationIssues().Should().HaveCount(1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldAllowSettingDimensionOrMaxDimension(bool setMaxDimension)
        {
            var image = new Image
            {
                Path = "path",
            };

            if (setMaxDimension)
            {
                image.MaxHeight = 1;
                image.MaxWidth = 1;
            }
            else
            {
                image.Height = 1;
                image.Width = 1;
            }

            image.GetValidationIssues().Should().BeEmpty();
        }
    }
}
