using System;
using System.IO;

namespace Pashua
{
    public sealed partial class TextBox : IHaveResults
    {
        /// <summary>
        /// Sets the initial contents. Newlines are automatically converted to the [return] token.
        /// </summary>
        public string Default { get; set; }

        partial void WriteSpecialProperties(StreamWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(Default))
            {
                writer.WriteLine($"{Id}.default = {Default.Replace("\n","[return]")};");
            }
        }

        /// <summary>
        /// The text the user entered with any returns turned into newlines. Set after the script is completed.
        /// </summary>
        public string EnteredText { get; private set; }

        void IHaveResults.SetResult(string result)
        {
            EnteredText = result.Replace("[return]","\n");
        }

        private static string SerializeEnum(FontSize size) => size.ToString().ToLowerInvariant();

        private static string SerializeEnum(FontType type) => type switch
            {
                FontType.Monospace => "fixed",
                _ => throw new InvalidOperationException("Unsupported font type."),
            };
    }
}
