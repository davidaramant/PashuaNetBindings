using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// Displays a checkbox
    /// </summary>
    public sealed partial class CheckBox : IPashuaControl
    {
        /// <summary>
        /// The name of this element in the Pashua script.  Should not be needed outside of the framework.
        /// </summary>
        public string Id => "checkbox" + GetHashCode();

        /// <summary>
        /// Creates a label / title next to the checkbox
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Set this to true if you want the checkbox to be checked by default.
        /// </summary>
        public bool Default { get; set; } = false;

        /// <summary>
        /// If set to true, the element will be disabled, so that the default value cannot be changed.
        /// </summary>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// String to use as tooltip for the button. Newlines are automatically escaped.
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
        public void WriteTo(TextWriter writer)
        {
            var errors = GetValidationIssues();
            if(errors.Any())
            {
                throw new PashuaScriptException(errors);
            }

            writer.WriteLine($"{Id}.type = checkbox");
            WriteSpecialProperties(writer);
            writer.WriteLine($"{Id}.label = {Label}");
            if (Default != false)
            {
                writer.WriteLine($"{Id}.default = {(Default ? 1 : 0)}");
            }
            if (Disabled != false)
            {
                writer.WriteLine($"{Id}.disabled = {(Disabled ? 1 : 0)}");
            }
            if (!string.IsNullOrWhiteSpace(Tooltip))
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip.Replace("\n", "\\n")}");
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
                errors.Add("CheckBox X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("CheckBox Y must be greater than or equal to 0.");
            }
            if (RelY <= -20)
            {
                errors.Add("CheckBox RelY must be greater than -20.");
            }
            if (string.IsNullOrWhiteSpace(Label))
            {
                errors.Add("CheckBox Label must be set.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
