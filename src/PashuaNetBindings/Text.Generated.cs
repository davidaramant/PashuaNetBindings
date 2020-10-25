using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// This element displays multi-line, wrapping text.
    /// </summary>
    /// <remarks>
    /// The width of the element does not adapt automatically to the content, but uses the given width (or the default
    /// width, if no width is specified.) On the other hand, the height is automatically set to the minimum height
    /// needed to display the text using the given (or default) width.
    /// </remarks>
    public sealed partial class Text : IPashuaControl
    {
        internal string Id => "text" + GetHashCode();

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sets the width of the text (in pixels)
        /// </summary>
        public int Width { get; set; } = 280;

        /// <summary>
        /// String to use as tooltip for the button. Use \n to insert a linebreak.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// The text to display. You can use the string [return] to insert a linebreak.
        /// </summary>
        public string Default { get; set; }

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

            writer.WriteLine($"{Id}.type = text");
            WriteSpecialProperties(writer);
            if (!string.IsNullOrWhiteSpace(Label))
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            if (Width != 280)
            {
                writer.WriteLine($"{Id}.width = {Width};");
            }
            if (!string.IsNullOrWhiteSpace(Tooltip))
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip};");
            }
            writer.WriteLine($"{Id}.default = {Default};");
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
                errors.Add("Text X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("Text Y must be greater than or equal to 0.");
            }
            if (Width < 0)
            {
                errors.Add("Text Width must be greater than or equal to 0.");
            }
            if (RelY <= -20)
            {
                errors.Add("Text RelY must be greater than -20.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
