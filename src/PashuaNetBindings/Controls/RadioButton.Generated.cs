using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// A radiobutton element lets the user choose a value from a pre-defined list of values.
    /// </summary>
    public sealed partial class RadioButton : IPashuaControl
    {
        /// <summary>
        /// The name of this element in the Pashua script.  Should not be needed outside of the framework.
        /// </summary>
        public string Id => "radiobutton" + GetHashCode();

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Any strings that should be used as a selectable value.
        /// </summary>
        public IEnumerable<string> Options { get; set; }

        /// <summary>
        /// The value that should be selected by default. Of course, this must be one of the option values to work.
        /// </summary>
        public string Default { get; set; }

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

            writer.WriteLine($"{Id}.type = radiobutton");
            WriteSpecialProperties(writer);
            if (!string.IsNullOrWhiteSpace(Label))
            {
                writer.WriteLine($"{Id}.label = {Label}");
            }
            foreach (var option in Options)
            {
                writer.WriteLine($"{Id}.option = {option}");
            }
            if (!string.IsNullOrWhiteSpace(Default))
            {
                writer.WriteLine($"{Id}.default = {Default}");
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
                errors.Add("RadioButton X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("RadioButton Y must be greater than or equal to 0.");
            }
            if (RelY <= -20)
            {
                errors.Add("RadioButton RelY must be greater than -20.");
            }
            if (Options == null || Options.Any(string.IsNullOrWhiteSpace))
            {
                errors.Add("RadioButton Options must be set and have at least one value.  Empty strings are not valid options.");
            }
            else if(!string.IsNullOrWhiteSpace(Default) && !Options.Contains(Default))
            {
                errors.Add("RadioButton Default must be one of the Options.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
