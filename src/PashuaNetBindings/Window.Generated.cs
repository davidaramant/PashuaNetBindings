using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// Customize the Pashua window.
    /// </summary>
    public sealed partial class Window : IPashuaControl
    {
        /// <summary>
        /// If set, the dialog will automatically close after the specified time has passed.
        /// </summary>
        /// <remarks>
        /// The timer starts in the very moment when Pashua has finished parsing the configuration string and
        /// everything is set up.
        /// </remarks>
        public TimeSpan? AutoCloseTime { get; set; }

        /// <summary>
        /// Can be used to preserve the window position between launches.
        /// </summary>
        /// <remarks>
        /// To let Pashua differentiate between applications, you have to set this to an arbitrary string. I.e.: one
        /// application can set this to “abc” and another one to “def”, and for both applications, the window position
        /// will be saved and restored separately.
        /// </remarks>
        public string AutoSaveKey { get; set; }

        /// <summary>
        /// Setting this to true will result in the window floating above other windows.
        /// </summary>
        public bool Floating { get; set; } = false;

        /// <summary>
        /// Sets the window title
        /// </summary>
        public string Title { get; set; } = "Pashua";

        /// <summary>
        /// Sets the window’s transparency, from 0 (invisible) to 1 (opaque)
        /// </summary>
        public double? Transparency { get; set; } = 1;

        /// <summary>
        /// Sets the horizontal position where the window should be opened on the screen (0 is the left border of the
        /// screen)
        /// </summary>
        /// <remarks>
        /// By default the window will be positioned automatically.
        /// </remarks>
        public int? X { get; set; }

        /// <summary>
        /// Sets the vertical position where the window should be opened on the screen (0 is the upper border of the
        /// screen)
        /// </summary>
        /// <remarks>
        /// By default the window will be positioned automatically.
        /// </remarks>
        public int? Y { get; set; }

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

            WriteSpecialProperties(writer);
            if (AutoCloseTime != null)
            {
                writer.WriteLine($"*.autoclosetime = {(int)AutoCloseTime?.TotalSeconds};");
            }
            if (AutoSaveKey != null)
            {
                writer.WriteLine($"*.autosavekey = {AutoSaveKey};");
            }
            if (Floating != false)
            {
                writer.WriteLine($"*.floating = {(Floating ? 1 : 0)};");
            }
            if (Title != "Pashua")
            {
                writer.WriteLine($"*.title = {Title};");
            }
            if (Transparency != 1)
            {
                writer.WriteLine($"*.transparency = {Transparency:N2};");
            }
            if (X != null)
            {
                writer.WriteLine($"*.x = {X};");
            }
            if (Y != null)
            {
                writer.WriteLine($"*.y = {Y};");
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
                errors.Add("Window X must be greater than or equal to 0.");
            }
            if (Y < 0)
            {
                errors.Add("Window Y must be greater than or equal to 0.");
            }
            AdditionalValidation(errors);
            return errors;
        }

        partial void AdditionalValidation(List<string> errors);
    }
}
