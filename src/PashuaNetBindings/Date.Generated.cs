using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// The date element lets the user choose a date, a time or both. It can be displayed in textual or graphical
    /// style.
    /// </summary>
    public sealed partial class Date : PashuaControl
    {
        internal string Id => "date" + GetHashCode();

        /// <summary>
        /// Sets the date picker’s label (displayed above)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Should the textual display style be used instead of the graphical style?
        /// </summary>
        public bool Textual { get; set; } = false;

        /// <summary>
        /// Should the user be able to chose a date?
        /// </summary>
        public bool AllowSettingDate { get; set; } = true;

        /// <summary>
        /// Should the user be able to chose the time?
        /// </summary>
        public bool AllowSettingTime { get; set; } = false;

        /// <summary>
        /// Default date and/or time that should be selected when the dialog is opened.
        /// </summary>
        public DateTime? Default { get; set; }

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
        public override void WriteTo(StreamWriter writer)
        {
            var errors = GetValidationIssues();
            if(errors.Any())
            {
                throw new PashuaScriptException(errors);
            }

            writer.WriteLine($"{Id}.type = date");
            if (Label != null)
            {
                writer.WriteLine($"{Id}.label = {Label};");
            }
            if (Textual != false)
            {
                writer.WriteLine($"{Id}.textual = {(Textual ? 1 : 0)};");
            }
            if (AllowSettingDate != true)
            {
                writer.WriteLine($"{Id}.date = {(AllowSettingDate ? 1 : 0)};");
            }
            if (AllowSettingTime != false)
            {
                writer.WriteLine($"{Id}.time = {(AllowSettingTime ? 1 : 0)};");
            }
            if (Default != null)
            {
                writer.WriteLine($"{Id}.default = {Default?.ToString("yyyy-mm-dd hh:mm")};");
            }
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
            if (Tooltip != null)
            {
                writer.WriteLine($"{Id}.tooltip = {Tooltip};");
            }
        }
    }
}
