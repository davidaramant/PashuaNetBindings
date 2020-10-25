using System;

namespace Pashua
{
    public sealed partial class TextBox
    {
        private static string SerializeEnum(FontSize size) => size.ToString().ToLowerInvariant();

        private static string SerializeEnum(FontType type) => type switch
            {
                FontType.Monospace => "fixed",
                _ => throw new InvalidOperationException("Unsupported font type."),
            };
    }
}
