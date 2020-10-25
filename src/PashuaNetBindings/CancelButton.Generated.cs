using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// cancelbutton can be triggered using Escape and closes the window without returning any values, except the
    /// cancelbutton’s own variable.
    /// </summary>
    /// <remarks>
    /// The cancel button is always positioned to the left of the default button and can not be moved to any other
    /// position.
    /// </remarks>
    public sealed partial class CancelButton : IPashuaControl
    {
        internal string Id => "cancelbutton" + GetHashCode();

        /// <summary>
        /// Sets the button title
        /// </summary>
        public string Label { get; set; }

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
            var errors = new List<string>();
            FindErrors(errors);
            if(errors.Any())
            {
                throw new PashuaScriptException(errors);
            }

            writer.WriteLine($"{Id}.type = cancelbutton");
            if (Label != null)
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            if (Disabled != false)
            {
                writer.WriteLine($"{Id}.disabled = {(Disabled ? 1 : 0)};");
            }
            if (Tooltip != null)
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip};");
            }
        }

        partial void FindErrors(List<string> validationErrors);
    }
}
