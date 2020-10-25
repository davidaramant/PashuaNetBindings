using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// An openbrowser is used for choosing a filesystem path.
    /// </summary>
    /// <remarks>
    /// It consists of a textfield, a button and (optionally) a label. The textfield holds the actual element value
    /// (the file path), while the button (which is localized) is used to invoke a file selector sheet. Moreover, a
    /// file can be dragged & dropped onto the textfield.
    /// </remarks>
    public sealed partial class OpenBrowser : IPashuaControl
    {
        internal string Id => "openbrowser" + GetHashCode();

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sets the width (overall width of texfield and button)
        /// </summary>
        public int Width { get; set; } = 300;

        /// <summary>
        /// Default path
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// File types that can be selected in the open dialog or dropped onto the textfield. In addition to filename
        /// extensions, you can use directory to let the user choose directories.
        /// </summary>
        /// <remarks>
        /// If only directory is specified, the user won’t be able to choose any files. If only filename extensions are
        /// specified, the user won’t be able to choose directories. If you don’t specify filetype at all, the user
        /// will be able to choose anything in the open dialog box.
        /// </remarks>
        public IEnumerable<string> FileTypes { get; set; }

        /// <summary>
        /// If set to true, input is mandatory.
        /// </summary>
        public bool Mandatory { get; set; } = false;

        /// <summary>
        /// If present, this string will be as the field’s placeholder string.
        /// </summary>
        public string Placeholder { get; set; }

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

            writer.WriteLine($"{Id}.type = openbrowser");
            if (Label != null)
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            if (Width != 300)
            {
                writer.WriteLine($"{Id}.width = {Width};");
            }
            if (Default != null)
            {
                writer.WriteLine($"{Id}.default = {Default};");
            }
            foreach (var option in FileTypes)
            {
                writer.WriteLine($"{Id}.filetype = {option};");
            }
            if (Mandatory != false)
            {
                writer.WriteLine($"{Id}.mandatory = {(Mandatory ? 1 : 0)};");
            }
            if (Placeholder != null)
            {
                writer.WriteLine($"{Id}.placeholder = {Placeholder};");
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

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        public IEnumerable<string> GetValidationIssues()
        {
            var errors = new List<string>();
            if (X < 0)
            {
                errors.Add("OpenBrowser X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("OpenBrowser Y must be greater than or equal to 0.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
