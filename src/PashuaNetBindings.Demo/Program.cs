using System;
using System.Collections.Generic;
using Pashua;
using Pashua.ScriptExtensions;

namespace PashuaNetBindings.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string lastOutput = string.Empty;

            while (lastOutput != null)
            {
                Page? page = ShowDemoSelection(lastOutput);

                lastOutput = page switch
                {
                    Page.Buttons => ShowButtonDemo(),
                    Page.CheckBoxes => ShowCheckBoxDemo(),
                    Page.ComboBoxes => ShowComboBoxDemo(),
                    _ => null
                };
            }
        }

        enum Page
        {
            Buttons,
            CheckBoxes,
            ComboBoxes,
        }

        static Page? ShowDemoSelection(string lastOutput)
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Control Demos" }
            };
            var option = script.AddAndReturn(
                new ComboBox
                {
                    Label = "Select demo page to view:",
                    Options = Enum.GetNames(typeof(Page)),
                });
            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });
            script.Add(new DefaultButton { Label = "Show Selected Page" });

            if (!string.IsNullOrWhiteSpace(lastOutput))
            {
                script.Add(new Text
                {
                    Label = "Output from last demo page:",
                    Default = lastOutput,
                });
            }

            script.Run();

            return Enum.TryParse(typeof(Page), option.SelectedOption, out object page) ? (Page?)page : null;
        }

        static string ShowButtonDemo()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Button Demos"},
                new Text { Default = "Various button functionality."},
                new Button { Label = "Has Tooltip", Tooltip = "Multiline\nTooltip"},
                new Button { Label ="Disabled", Disabled = true},
                new DefaultButton { Label = "Return to Demo List" },
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : string.Empty;
        }

        static string ShowCheckBoxDemo()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "CheckBox Demos"},
                new DefaultButton { Label = "Return to Demo List" },
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            var defaultCb = script.AddAndReturn(new CheckBox { Label = "Default option", Default = true });
            var withTooltip = script.AddAndReturn(new CheckBox { Label = "Has Tooltip", Tooltip = "A tooltip!" });
            script.Add(new CheckBox { Label = "Disabled", Disabled = true });

            script.Run();

            if (cancel.WasClicked)
                return null;

            if (defaultCb.WasChecked)
                return "Default checkbox was selected";

            return "Checkbox with tooltip was selected";
        }

        static string ShowComboBoxDemo()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "ComboBox Demos"},
                new DefaultButton { Label = "Return to Demo List" },
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            var noCompletion = script.AddAndReturn(new ComboBox
            {
                Label = "No Completion - will be returned",
                Options = new []{"A","B","C"},
                Completion = AutoCompletionMode.None,
                Placeholder = "Placeholder text",
                Width = 500,
            });
            var caseInsensitive = script.AddAndReturn(new ComboBox
            {
                Label = "Case-Insensitive Completion",
                Options = new []{"A","B","C"},
                Completion = AutoCompletionMode.CaseSensitive,
                Placeholder = "Placeholder text",
                Rows = 4,
            });

            script.Run();

            if (cancel.WasClicked)
                return null;

            return noCompletion.SelectedOption;
        }
    }
}