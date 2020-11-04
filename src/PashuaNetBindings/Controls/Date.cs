using System;
using System.Globalization;
using System.IO;

namespace Pashua
{
    public sealed partial class Date : IHaveResults
    {
        /// <summary>
        /// Is the user allowed to select the date, time, or both?
        /// </summary>
        public DateTimeSelection SelectionMode { get; set; } = DateTimeSelection.DateOnly;

        /// <summary>
        /// Called when the script is completed.
        /// </summary>
        public Action<DateTime> TimestampChosen { get; set; }

        void IHaveResults.SetResult(string result)
        {
            var timestamp = SelectionMode switch
            {
                DateTimeSelection.DateOnly => DateTime.ParseExact(result, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateTimeSelection.TimeOnly => DateTime.MinValue.Add(DateTime.ParseExact(result, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay),
                DateTimeSelection.BothTimeAndDate => DateTime.ParseExact(result, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                _ => throw new InvalidOperationException("Unknown DateTimeSelection mode")
            };
            TimestampChosen?.Invoke(timestamp);
        }

        partial void WriteSpecialProperties(TextWriter writer)
        {
            switch (SelectionMode)
            {
                case DateTimeSelection.DateOnly:
                    // do nothing; default case
                    break;

                case DateTimeSelection.TimeOnly:
                    writer.WriteLine($"{Id}.date = 0");
                    writer.WriteLine($"{Id}.time = 1");
                    break;

                case DateTimeSelection.BothTimeAndDate:
                    writer.WriteLine($"{Id}.time = 1");
                    break;

                default:
                    throw new InvalidOperationException("Unknown value for SelectionMode");
            }
        }
    }
}
