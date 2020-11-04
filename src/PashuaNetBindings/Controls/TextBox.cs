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

        partial void WriteSpecialProperties(TextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(Default))
            {
                writer.WriteLine($"{Id}.default = {Default.Replace("\n","[return]")}");
            }
        }

        /// <summary>
        /// Called when the script completes.  The argument will the be the text the user entered (which may be empty), with any returns turned into newlines.
        /// </summary>
        public Action<string> TextEntered { get; set; }

        void IHaveResults.SetResult(string result) => TextEntered?.Invoke(result.Replace("[return]","\n"));

        private static string SerializeEnum(FontSize size) => size.ToString().ToLowerInvariant();

        private static string SerializeEnum(FontType type) => type switch
            {
                FontType.Monospace => "fixed",
                _ => throw new InvalidOperationException("Unsupported font type."),
            };
    }
}
