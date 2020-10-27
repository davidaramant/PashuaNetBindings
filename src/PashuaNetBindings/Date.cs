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
        /// The date and/or time the user selected.
        /// </summary>
        public DateTime SelectedTimestamp { get; private set; }

        void IHaveResults.SetResult(string result)
        {
            SelectedTimestamp = SelectionMode switch
            {
                DateTimeSelection.DateOnly => DateTime.ParseExact(result, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateTimeSelection.TimeOnly => DateTime.MinValue.Add(DateTime.ParseExact(result, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay),
                DateTimeSelection.BothTimeAndDate => DateTime.ParseExact(result, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                _ => throw new InvalidOperationException("Unknown DateTimeSelection mode")
            };
        }

        partial void WriteSpecialProperties(StreamWriter writer)
        {
            switch (SelectionMode)
            {
                case DateTimeSelection.DateOnly:
                    // do nothing; default case
                    break;

                case DateTimeSelection.TimeOnly:
                    writer.WriteLine($"{Id}.date = 0;");
                    writer.WriteLine($"{Id}.time = 1;");
                    break;

                case DateTimeSelection.BothTimeAndDate:
                    writer.WriteLine($"{Id}.time = 1;");
                    break;

                default:
                    throw new InvalidOperationException("Unknown value for SelectionMode");
            }
        }
    }
}
