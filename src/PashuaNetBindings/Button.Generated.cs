using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// A clickable button.
    /// </summary>
    /// <remarks>
    /// A button works similar to the default button: when clicked, it closes the window and returns all elements’
    /// values, but additionally, the button’s own value is returned as true. Buttons (“regular” buttons, not the
    /// cancel button or the default button) are always positioned in the lower left area of the window, though you can
    /// position them absolutely using attributes x and y
    /// </remarks>
    public sealed partial class Button : IPashuaControl
    {
        /// <summary>
        /// The name of this element in the Pashua script.  Should not be needed outside of the framework.
        /// </summary>
        public string Id => "button" + GetHashCode();

        /// <summary>
        /// Sets the button’s text
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Absolute horizontal position in the window, measured from the left border of the content area
        /// </summary>
        public int? X { get; set; }

        /// <summary>
        /// Absolute vertical position in the window, measured from the lower border of the content area
        /// </summary>
        public int? Y { get; set; }

        /// <summary>
        /// If set to true, the element will be disabled, so that the default value cannot be changed.
        /// </summary>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// String to use as tooltip for the button. Use \n to insert a linebreak.
        /// </summary>
        public string Tooltip { get; set; }

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

            writer.WriteLine($"{Id}.type = button");
            WriteSpecialProperties(writer);
            writer.WriteLine($"{Id}.label = {Label};");
            if (X != null)
            {
                writer.WriteLine($"{Id}.x = {X};");
            }
            if (Y != null)
            {
                writer.WriteLine($"{Id}.y = {Y};");
            }
            if (Disabled != false)
            {
                writer.WriteLine($"{Id}.disabled = {(Disabled ? 1 : 0)};");
            }
            if (!string.IsNullOrWhiteSpace(Tooltip))
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip};");
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
                errors.Add("Button X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("Button Y must be greater than or equal to 0.");
            }
            if (string.IsNullOrWhiteSpace(Label))
            {
                errors.Add("Button Label must be set.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
