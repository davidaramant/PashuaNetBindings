using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// A textbox is a scrollable, multi-line text container.
    /// </summary>
    /// <remarks>
    /// The scrollbar will appear automatically if needed. Note: If you have changed the system’s scrollbar behaviour
    /// to display both arrows at both ends (e.g. using TinkerTool), the scrollbar might not appear for small heights.
    /// </remarks>
    public sealed partial class TextBox : IPashuaControl
    {
        internal string Id => "textbox" + GetHashCode();

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sets the width of the textbox (in pixels)
        /// </summary>
        public int Width { get; set; } = 250;

        /// <summary>
        /// Sets the height of the text (in pixels)
        /// </summary>
        public int Height { get; set; } = 52;

        /// <summary>
        /// Sets the initial contents. You can use the string [return] to insert a linebreak.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Size of the text inside the textbox. There are three sizes available, which conform to the system’s
        /// standard sizes: regular, small, or mini.
        /// </summary>
        public FontSize FontSize { get; set; } = FontSize.Regular;

        /// <summary>
        /// Set this to control if the text should be displayed using a monospace font.
        /// </summary>
        public FontType FontType { get; set; } = FontType.Proportional;

        /// <summary>
        /// If set to true, input is mandatory.
        /// </summary>
        public bool Mandatory { get; set; } = false;

        /// <summary>
        /// If set to true, the element will be disabled, so that the default value cannot be changed.
        /// </summary>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// String to use as tooltip for the button. Use \n to insert a linebreak.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Absolute horizontal position in the window, measured from the left border of the content area
        /// </summary>
        public int? X { get; set; }

        /// <summary>
        /// Absolute vertical position in the window, measured from the lower border of the content area
        /// </summary>
        public int? Y { get; set; }

        /// <summary>
        /// Horizontal offset, relative to the position the element would have if RelX was not used (e.g.: RelX
        /// specifies the distance from the left window border). Any integer can be used as RelX value.
        /// </summary>
        public int RelX { get; set; } = 0;

        /// <summary>
        /// Relative vertical distance to the next element below (“relative” means that the value is added to the
        /// default distance). Any integer larger than -20 can be used as RelY value.
        /// </summary>
        public int RelY { get; set; } = 0;

        /// <summary>
        /// Writes the control script to the given writer.
        /// </summary>
        /// <exception cref="PashuaScriptException">Thrown if the control was not configured correctly.</exception>
        public void WriteTo(StreamWriter writer)
        {
            var errors = GetValidationIssues();
            if(errors.Any())
            {
                throw new PashuaScriptException(errors);
            }

            writer.WriteLine($"{Id}.type = textbox");
            WriteSpecialProperties(writer);
            if (Label != null)
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            if (Width != 250)
            {
                writer.WriteLine($"{Id}.width = {Width};");
            }
            if (Height != 52)
            {
                writer.WriteLine($"{Id}.height = {Height};");
            }
            if (Default != null)
            {
                writer.WriteLine($"{Id}.default = {Default};");
            }
            if (FontSize != FontSize.Regular)
            {
                writer.WriteLine($"{Id}.fontsize = {SerializeEnum(FontSize)};");
            }
            if (FontType != FontType.Proportional)
            {
                writer.WriteLine($"{Id}.fonttype = {SerializeEnum(FontType)};");
            }
            if (Mandatory != false)
            {
                writer.WriteLine($"{Id}.mandatory = {(Mandatory ? 1 : 0)};");
            }
            if (Disabled != false)
            {
                writer.WriteLine($"{Id}.disabled = {(Disabled ? 1 : 0)};");
            }
            if (Tooltip != null)
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip};");
            }
            if (X != null)
            {
                writer.WriteLine($"{Id}.x = {X};");
            }
            if (Y != null)
            {
                writer.WriteLine($"{Id}.y = {Y};");
            }
            if (RelX != 0)
            {
                writer.WriteLine($"{Id}.relx = {RelX};");
            }
            if (RelY != 0)
            {
                writer.WriteLine($"{Id}.rely = {RelY};");
            }
        }

        partial void WriteSpecialProperties(StreamWriter writer);

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        public IEnumerable<string> GetValidationIssues()
        {
            var errors = new List<string>();
            if (X < 0)
            {
                errors.Add("TextBox X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("TextBox Y must be greater than or equal to 0.");
            }
            if (Width < 0)
            {
                errors.Add("TextBox Width must be greater than or equal to 0.");
            }
            if (RelY <= -20)
            {
                errors.Add("TextBox RelY must be greater than -20.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
