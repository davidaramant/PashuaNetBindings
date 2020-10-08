using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Pashua.Backend
{
    public sealed class DialogBuilder
    {
        private readonly Random _random = new Random();
        private readonly List<string> _script = new List<string>();

        internal DialogBuilder(IEnumerable<string> script)
        {
            _script.AddRange(script);
        }

        private string GetRandomId()
        {
            return new string(
                Enumerable.Range(0, 20).Select(_ => (char) _random.Next('a', 'z')).ToArray());
        }

        private ControlContext CreateControl(string type, string id = null)
        {
            return new ControlContext(_script, id, type);
        }

        public DialogBuilder WithButton(string id,
            string label,
            int? x = null,
            int? y = null,
            bool? enabled = null,
            string tooltip = null)
        {
            var control = CreateControl("button", id);
            control.Set("label", label);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            return this;
        }

        public DialogBuilder WithCancelButton(string id = "cancel",
            string label = null,
            bool? enabled = null,
            string tooltip = null)
        {
            var control = CreateControl("cancelbutton", id);
            control.Set("label", label);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            return this;
        }

        public DialogBuilder WithCheckBox(string id,
            string label,
            bool? enabled = null,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("checkbox", id);
            control.Set("label", label);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relX", relativeX);
            control.Set("relY", relativeY);
            return this;
        }

        public DialogBuilder WithComboBox(string id,
            string label,
            IEnumerable<string> options,
            string defaultOption = null,
            AutoCompletion completion = AutoCompletion.CaseSensitive,
            bool? enabled = null,
            string tooltip = null,
            int? width = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("combobox", id);
            control.Set("label", label);
            foreach (var option in options) control.Set("option", option);
            control.Set("default", defaultOption);
            if (completion != AutoCompletion.CaseSensitive)
                control.Set("completion", ((int) completion).ToString(CultureInfo.InvariantCulture));
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("width", width);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relX", relativeX);
            control.Set("relY", relativeY);
            return this;
        }

        public DialogBuilder WithDate(string id,
            bool textual = false,
            bool chooseDate = true,
            bool chooseTime = false,
            DateTime? defaultDateTime = null,
            int? x = null,
            int? y = null,
            bool? enabled = false,
            string tooltip = null)
        {
            var control = CreateControl("date", id);
            control.Set("textual", textual, false);
            control.Set("date", chooseDate, true);
            control.Set("time", chooseTime, false);
            if (defaultDateTime.HasValue) control.Set("default", defaultDateTime.Value.ToString("yyyy-MM-dd HH:mm"));
            control.Set("x", x);
            control.Set("y", y);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            return this;
        }

        public DialogBuilder WithDefaultButton(string id = null,
            string label = null,
            bool? enabled = null,
            string tooltip = null)
        {
            var control = CreateControl("defaultbutton", id);
            control.Set("label", label);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            return this;
        }

        public DialogBuilder WithImage(string path,
            string id = null,
            string label = null,
            bool border = false,
            int? maxWidth = null,
            int? maxHeight = null,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("image", id);
            control.Set("path", path);
            control.Set("label", label);
            control.Set("border", border, false);
            control.Set("maxwidth", maxWidth);
            control.Set("maxheight", maxHeight);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithOpenBrowser(string id,
            string label = null,
            int? width = null,
            string defaultPath = null,
            IEnumerable<string> fileTypes = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("openbrowser", id);
            control.Set("label", label);
            control.Set("width", width);
            control.Set("default", defaultPath);
            if (fileTypes != null) control.Set("filetype", string.Join(" ", fileTypes));
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithPassword(string id,
            string label = null,
            int? width = null,
            string defaultText = null,
            bool enabled = true,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("password", id);
            control.Set("label", label);
            control.Set("width", width);
            control.Set("default", defaultText);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithPopup(string id,
            IEnumerable<string> options,
            string defaultValue = null,
            string label = null,
            bool enabled = true,
            string tooltip = null,
            int? width = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("popup", id);
            control.Set("label", label);
            foreach (var option in options) control.Set("option", option);
            control.Set("default", defaultValue);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("width", width);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithRadioButtons(string id,
            IEnumerable<string> options,
            string defaultValue = null,
            string label = null,
            bool enabled = true,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("radiobutton", id);
            control.Set("label", label);
            foreach (var option in options) control.Set("option", option);
            control.Set("default", defaultValue);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithSaveBrowser(string id,
            string label = null,
            int? width = null,
            string defaultPath = null,
            string fileType = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("savebrowser", id);
            control.Set("label", label);
            control.Set("width", width);
            control.Set("default", defaultPath);
            control.Set("tooltip", fileType);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithText(string text,
            string id = null,
            string label = null,
            int? width = null,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("text", id);
            control.Set("text", EscapeNewLines(text));
            control.Set("label", label);
            control.Set("width", width);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithTextBox(string id,
            string label = null,
            int? width = null,
            int? height = null,
            string defaultText = null,
            FontSize fontSize = FontSize.Regular,
            bool monospaceFont = false,
            bool enabled = true,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("textbox", id);
            control.Set("label", label);
            control.Set("width", width);
            control.Set("height", height);
            control.Set("default", EscapeNewLines(defaultText));
            switch (fontSize)
            {
                case FontSize.Mini:
                    control.Set("fontsize", "mini");
                    break;
                case FontSize.Small:
                    control.Set("fontsize", "small");
                    break;
                case FontSize.Regular:
                default:
                    // Do nothing
                    break;
            }

            if (monospaceFont) control.Set("fonttype", "fixed");
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        public DialogBuilder WithTextField(string id,
            string label = null,
            int? width = null,
            string defaultText = null,
            bool enabled = true,
            string tooltip = null,
            int? x = null,
            int? y = null,
            int? relativeX = null,
            int? relativeY = null)
        {
            var control = CreateControl("textfield", id);
            control.Set("label", label);
            control.Set("width", width);
            control.Set("default", defaultText);
            control.Set("disabled", !enabled, false);
            control.Set("tooltip", tooltip);
            control.Set("x", x);
            control.Set("y", y);
            control.Set("relx", relativeX);
            control.Set("rely", relativeY);
            return this;
        }

        private static string EscapeNewLines(string text)
        {
            if (text == null) return null;
            return text.Replace("\n", "[return]");
        }

        public void Show()
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pashua.app",
                    "Contents", "MacOS", "Pashua"),
                Arguments = "-",
                RedirectStandardInput = true
            };

            using (var process = Process.Start(startInfo))
            {
                foreach (var line in _script) process.StandardInput.WriteLine(line);
                process.StandardInput.Close();
                process.WaitForExit();
            }
        }
    }
}