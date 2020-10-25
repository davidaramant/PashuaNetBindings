using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// This element displays an image (or a PDF file), optionally scaling it to a maximum width or height.
    /// </summary>
    /// <remarks>
    /// Pashua can handle any image type that is supported by NSImage. This includes TIFF, GIF, JPEG, PNG, PDF, PICT
    /// and EPS.
    /// </remarks>
    public sealed partial class Image : IPashuaControl
    {
        internal string Id => "image" + GetHashCode();

        /// <summary>
        /// Creates a label above this element
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Filesystem path to the image
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Set this to true to display a border for the image
        /// </summary>
        public bool Border { get; set; } = false;

        /// <summary>
        /// If this attribute is set to some integer number, the image’s width will be set to this width (including
        /// border)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// If this attribute is set to some integer number, the image’s height will be set to this value (including
        /// border)
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// If this attribute is set to some integer number, the image will be scaled down to this width (including
        /// border), if it’s wider
        /// </summary>
        /// <remarks>
        /// Default is calculated from window’s width
        /// </remarks>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// If this attribute is set to some integer number, the image will be scaled down to this height (including
        /// border), if it’s higher
        /// </summary>
        /// <remarks>
        /// Default is calculated from window’s height
        /// </remarks>
        public int? MaxHeight { get; set; }

        /// <summary>
        /// Set this to true to allow the image to be upscaled (which will only have an effect if setting width or
        /// height)
        /// </summary>
        public bool Upscale { get; set; } = false;

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

            writer.WriteLine($"{Id}.type = image");
            if (Label != null)
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            writer.WriteLine($"{Id}.path = {Path};");
            if (Border != false)
            {
                writer.WriteLine($"{Id}.border = {(Border ? 1 : 0)};");
            }
            if (Width != null)
            {
                writer.WriteLine($"{Id}.width = {Width};");
            }
            if (Height != null)
            {
                writer.WriteLine($"{Id}.height = {Height};");
            }
            if (MaxWidth != null)
            {
                writer.WriteLine($"{Id}.maxwidth = {MaxWidth};");
            }
            if (MaxHeight != null)
            {
                writer.WriteLine($"{Id}.maxheight = {MaxHeight};");
            }
            if (Upscale != false)
            {
                writer.WriteLine($"{Id}.upscale = {(Upscale ? 1 : 0)};");
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

        /// <summary>
        /// Returns all the validation errors with the control.
        /// </summary>
        /// <returns>All the issues.</returns>
        public IEnumerable<string> GetValidationIssues()
        {
            var errors = new List<string>();
            if (X < 0)
            {
                errors.Add("Image X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("Image Y must be greater than or equal to 0.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
