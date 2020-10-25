using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// When the default button is pressed, the window is closed and all elements’ values are returned to the calling
    /// script. The default button is always located in the lower right corner of the window and can’t be moved to any
    /// other position.
    /// </summary>
    /// <remarks>
    /// A default button is added to each window automatically – you only have to specify it explicitly, if you want to
    /// set the label or a tooltip or need the return value (i.e.: has it been clicked?) of this button.
    /// </remarks>
    public sealed partial class DefaultButton : IPashuaControl
    {
        internal string Id => "defaultbutton" + GetHashCode();

        /// <summary>
        /// Sets the button title
        /// </summary>
        public string Label { get; set; } = "OK";

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

            writer.WriteLine($"{Id}.type = defaultbutton");
            if (Label != "OK")
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
