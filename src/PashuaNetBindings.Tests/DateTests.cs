using FluentAssertions;
using System;
using Xunit;

namespace Pashua.Tests
{
    public class DateTests
    {
        [Theory]
        [InlineData("12:34", 12, 34)]
        [InlineData("21:56", 21, 56)]
        public void ShouldParseTimeResult(string rawResult, int hours, int minutes)
        {
            var timestamp = new DateTime();

            var date = new Date
            {
                SelectionMode = DateTimeSelection.TimeOnly,
                TimestampChosen = dt => timestamp = dt,
            };
            ((IHaveResults)date).SetResult(rawResult);

            var expectedDateTime = DateTime.MinValue.AddHours(hours).AddMinutes(minutes);
            timestamp.Should().Be(expectedDateTime);
        }

        [Theory]
        [InlineData("2020-01-02", 2020, 1, 2)]
        [InlineData("2025-12-31", 2025, 12, 31)]
        public void ShouldParseDateResult(string rawResult, int year, int month, int day)
        {
            var timestamp = new DateTime();

            var date = new Date
            {
                SelectionMode = DateTimeSelection.DateOnly,
                TimestampChosen = dt => timestamp = dt,
            };
            ((IHaveResults)date).SetResult(rawResult);

            var expectedDateTime = new DateTime(year, month, day);
            timestamp.Should().Be(expectedDateTime);
        }


        [Theory]
        [InlineData("2020-01-02 12:34", 2020, 1, 2, 12, 34)]
        [InlineData("2025-12-31 00:00", 2025, 12, 31, 0, 0)]
        public void ShouldParseDateTimeResult(string rawResult, int year, int month, int day, int hours, int minutes)
        {
            var timestamp = new DateTime();

            var date = new Date
            {
                SelectionMode = DateTimeSelection.BothTimeAndDate,
                TimestampChosen = dt => timestamp = dt,
            };
            ((IHaveResults)date).SetResult(rawResult);

            var expectedDateTime = new DateTime(year, month, day, hours, minutes, 0);
            timestamp.Should().Be(expectedDateTime);
        }
    }
}
