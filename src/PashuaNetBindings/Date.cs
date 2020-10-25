using System;
using System.IO;

namespace Pashua
{
    public sealed partial class Date
    {
        /// <summary>
        /// Is the user allowed to select the date, time, or both?
        /// </summary>
        public DateTimeSelection SelectionMode { get; set; } = DateTimeSelection.DateOnly;

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
