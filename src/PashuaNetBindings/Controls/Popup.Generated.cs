using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// A popup is an element that lets the user choose one value from a list of possible values
    /// </summary>
    public sealed partial class Popup : IPashuaControl
    {
        /// <summary>
        /// The name of this element in the Pashua script.  Should not be needed outside of the framework.
        /// </summary>
        public string Id => "popup" + GetHashCode();

        /// <summary>
        /// Any strings that should appear as an entry in the popup
        /// </summary>
        public IEnumerable<string> Options { get; set; }

        /// <summary>
        /// Default value (should match one of the option attributes)
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// If set to true, the element will be disabled, so that the default value cannot be changed.
        /// </summary>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// String to use as tooltip for the button. Newlines are automatically escaped.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// If set to true, input is mandatory.
        /// </summary>
        public bool Mandatory { get; set; } = false;

        /// <summary>
        /// Width in pixels
        /// </summary>
        public int Width { get; set; } = 200;

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
        public void WriteTo(TextWriter writer)
        {
            var errors = GetValidationIssues();
            if(errors.Any())
            {
                throw new PashuaScriptException(errors);
            }

            writer.WriteLine($"{Id}.type = popup");
            WriteSpecialProperties(writer);
            foreach (var option in Options)
            {
                writer.WriteLine($"{Id}.option = {option}");
            }
            if (!string.IsNullOrWhiteSpace(Default))
            {
                writer.WriteLine($"{Id}.default = {Default}");
            }
            if (!string.IsNullOrWhiteSpace(Label))
            {
                writer.WriteLine($"{Id}.label = {Label}");
            }
            if (Disabled != false)
            {
                writer.WriteLine($"{Id}.disabled = {(Disabled ? 1 : 0)}");
            }
            if (!string.IsNullOrWhiteSpace(Tooltip))
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip.Replace("\n", "\\n")}");
            }
            if (Mandatory != false)
            {
                writer.WriteLine($"{Id}.mandatory = {(Mandatory ? 1 : 0)}");
            }
            if (Width != 200)
            {
                writer.WriteLine($"{Id}.width = {Width}");
            }
            if (X != null)
            {
                writer.WriteLine($"{Id}.x = {X}");
            }
            if (Y != null)
            {
                writer.WriteLine($"{Id}.y = {Y}");
            }
            if (RelX != 0)
            {
                writer.WriteLine($"{Id}.relx = {RelX}");
            }
            if (RelY != 0)
            {
                writer.WriteLine($"{Id}.rely = {RelY}");
            }
        }

        partial void WriteSpecialProperties(TextWriter writer);

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        public IEnumerable<string> GetValidationIssues()
        {
            var errors = new List<string>();
            if (X < 0)
            {
                errors.Add("Popup X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("Popup Y must be greater than or equal to 0.");
            }
            if (Width < 0)
            {
                errors.Add("Popup Width must be greater than or equal to 0.");
            }
            if (RelY <= -20)
            {
                errors.Add("Popup RelY must be greater than -20.");
            }
            if (Options == null || Options.Any(string.IsNullOrWhiteSpace))
            {
                errors.Add("Popup Options must be set and have at least one value.  Empty strings are not valid options.");
            }
            else if(!string.IsNullOrWhiteSpace(Default) && !Options.Contains(Default))
            {
                errors.Add("Popup Default must be one of the Options.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
