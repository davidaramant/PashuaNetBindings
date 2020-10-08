using System.Collections.Generic;

namespace Pashua.ListExtensions
{
    public static class Extensions
    {
        public static void AddFormat(this List<string> builder, string format, params object[] args)
        {
            builder.Add(string.Format(format, args));
        }
    }
}