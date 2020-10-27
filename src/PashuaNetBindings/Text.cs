using System.Collections.Generic;
using System.IO;

namespace Pashua
{
    public sealed partial class Text
    {
        /// <summary>
        /// Sets the initial contents. Newlines are automatically converted to the [return] token.
        /// </summary>
        public string Default { get; set; }

        partial void WriteSpecialProperties(StreamWriter writer)
        {
            writer.WriteLine($"{Id}.default = {Default.Replace("\n", "[return]")};");
        }

        partial void AdditionalValidation(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(Default))
            {
                errors.Add("Text Default must be set.");
            }
        }
    }
}
