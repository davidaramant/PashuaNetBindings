using System;
using System.Collections.Generic;
using System.IO;
using Pashua;

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
                    Page.Button => ShowButtonDemo(),
                    Page.CheckBox => ShowCheckBoxDemo(),
                    Page.ComboBox => ShowComboBoxDemo(),
                    Page.Date => ShowDateDemos(),
                    Page.Image => ShowImageDemos(),
                    Page.OpenBrowser => ShowOpenBrowserDemos(),
                    Page.Password => ShowPasswordDemos(),
                    Page.Popup => ShowPopupDemos(),
                    Page.SaveBrowser => ShowSaveBrowserDemos(),
                    Page.Text => ShowTextDemos(),
                    Page.TextBox => ShowTextBoxDemos(),
                    Page.TextField => ShowTextFieldDemos(),
                    Page.Window => ShowWindowDemo(),
                    _ => null
                };
            }
        }

        enum Page
        {
            Button,
            CheckBox,
            ComboBox,
            Date,
            Image,
            OpenBrowser,
            Password,
            Popup,
            SaveBrowser,
            Text,
            TextBox,
            TextField,
            Window,
        }

        static Page? ShowDemoSelection(string lastOutput)
        {
            Page? selectedPage = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Control Demos" },
                new Text { Default = "This page also serves as the RadioButton demo" },
                new CancelButton { Label = "Quit" },
                new DefaultButton { Label = "Show Selected Page" },
                new RadioButton
                {
                    Label = "Select demo page to view:",
                    Options = Enum.GetNames(typeof(Page)),
                    OptionSelected = p =>
                    {
                        if (Enum.TryParse<Page>(p, out var page))
                        {
                            selectedPage = page;
                        }
                    },
                }
            };


            if (!string.IsNullOrWhiteSpace(lastOutput))
            {
                script.Add(new Text
                {
                    Label = "Output from last demo page:",
                    Default = lastOutput,
                });
            }

            script.RunScript();

            return selectedPage;
        }

        static string ShowButtonDemo()
        {
            bool canceled = false;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Button Demos"},
                new Text { Default = "Various button functionality."},
                new Button { Label = "Has Tooltip", Tooltip = "Multiline\nTooltip"},
                new Button { Label ="Disabled", Disabled = true},
                new DefaultButton { Label = "Return to Demo List" },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true,}
            };

            script.RunScript();

            return canceled ? null : string.Empty;
        }

        static string ShowCheckBoxDemo()
        {
            bool cancelClicked = false;
            var boxesChecked = new List<string>();

            var script = new List<IPashuaControl>
            {
                new Window { Title = "CheckBox Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new CancelButton { Label = "Quit", Clicked = ()=>cancelClicked = true},
                new CheckBox
                {
                    Label = "Default option",
                    Default = true,
                    Checked = () => boxesChecked.Add("Default"),
                },
                new CheckBox
                {
                    Label = "Has Tooltip",
                    Tooltip = "A tooltip!",
                    Checked = () => boxesChecked.Add("Tooltip"),
                },
                new CheckBox { Label = "Disabled", Disabled = true }
            };


            script.RunScript();

            return cancelClicked ? null : "Boxes checked: " + string.Join(", ", boxesChecked);
        }

        static string ShowComboBoxDemo()
        {
            bool canceled = false;
            string selectedOption = "";

            var script = new List<IPashuaControl>
            {
                new Window { Title = "ComboBox Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
                new ComboBox
                {
                    Label = "No Completion - will be returned",
                    Options = new[] { "A", "B", "C" },
                    Completion = AutoCompletionMode.None,
                    Placeholder = "Placeholder text",
                    Width = 500,
                    OptionSelected = o => selectedOption = o,
                },
                new ComboBox
                {
                    Label = "Case-Insensitive Completion",
                    Options = new[] { "A", "B", "C" },
                    Completion = AutoCompletionMode.CaseSensitive,
                    Rows = 4,
                },
            };

            script.RunScript();

            return canceled ? null : selectedOption;
        }

        static string ShowDateDemos()
        {
            bool canceled = false;
            DateTime chosenTimestamp = new DateTime();

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Date Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Date
                {
                    Label = "Date Only",
                    SelectionMode = DateTimeSelection.DateOnly,
                    Default = new DateTime(1984,4,4),
                },
                new Date
                {
                    Label = "Time Only",
                    SelectionMode = DateTimeSelection.TimeOnly,
                },
                new Date
                {
                    Label = "Date & Time - Textual",
                    SelectionMode = DateTimeSelection.BothTimeAndDate,
                    Textual = true,
                },
                new Date
                {
                    Label = "Date & Time - Will be returned",
                    SelectionMode = DateTimeSelection.BothTimeAndDate,
                    TimestampChosen = dt => chosenTimestamp = dt,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : chosenTimestamp.ToString("s");
        }

        static string ShowImageDemos()
        {
            var imgPath = 
                Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "test.png");

            bool canceled = false;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Image Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Image
                {
                    Label = "Default Options",
                    Path = imgPath,
                },
                new Image
                {
                    Label = "Border & Size Adjustment",
                    Path = imgPath,
                    Border = true,
                    Width = 160,
                    Height = 90,
                    Upscale = true,
                },
                new Image
                {
                    Label = "Max Dimensions (bug in Pashua?)",
                    Path = imgPath,
                    MaxWidth = 10,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : "";
        }

        static string ShowOpenBrowserDemos()
        {
            bool canceled = false;
            string selectedPath = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "OpenBrowser Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new OpenBrowser
                {
                    Label = "PNGs only",
                    Default = "test.png",
                    FileTypes = new []{"png"},
                },
                new OpenBrowser
                {
                    Label = "Directories",
                    FileTypes = new []{"directory"},
                },
                new OpenBrowser
                {
                    Label = "Will be returned",
                    PathSelected = p => selectedPath = p,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : selectedPath;
        }

        static string ShowPasswordDemos()
        {
            bool canceled = false;
            string password = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Password Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Password
                {
                    Label = "With default",
                    Default = "password1",
                },
                new Password
                {
                    Label = "Disabled",
                    Disabled = true,
                },
                new Password
                {
                    Label = "Will be returned",
                    TextEntered = p=>password = p,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : password;
        }

        static string ShowPopupDemos()
        {
            bool canceled = false;
            string selectedOption = "";

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Popup Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Popup
                {
                    Label = "With default",
                    Default = "A",
                    Options = new []{"A","B","C"}
                },
                new Popup
                {
                    Label = "Will be returned",
                    Options = new[] { "A", "B", "C" },
                    OptionSelected = o => selectedOption = o,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : selectedOption;
        }

        static string ShowSaveBrowserDemos()
        {
            bool canceled = false;
            string selectedPath = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "SaveBrowser Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new SaveBrowser
                {
                    Label = "PNGs only",
                    Default = "test.png",
                    Filetype = "png"
                },
                new SaveBrowser
                {
                    Label = "Will be returned",
                    PathSelected = p => selectedPath = p,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : selectedPath;
        }

        static string ShowTextDemos()
        {
            bool canceled = false;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "Text Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Text
                {
                    Label = "This is a label, which is different than the main text",
                    Default = "The text\ncan\nhave\nnewlines",
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : string.Empty;
        }

        static string ShowTextBoxDemos()
        {
            bool canceled = false;
            string text = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "TextBox Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new TextBox
                {
                    Label = "With default & small text",
                    Default = "default text\nwhich can be multiple lines",
                    FontSize = FontSize.Small,
                },
                new TextBox
                {
                    Label = "Mini, MonoType, Disabled",
                    FontSize = FontSize.Mini,
                    FontType = FontType.Monospace,
                    Disabled = true,
                    Default = "Here is some text to see how it looks",
                },
                new TextBox
                {
                    Label = "Will be returned",
                    TextEntered = t => text = t,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : text;
        }

        static string ShowTextFieldDemos()
        {
            bool canceled = false;
            string text = null;

            var script = new List<IPashuaControl>
            {
                new Window { Title = "TextField Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new TextField
                {
                    Label = "With default",
                    Default = "value",
                },
                new TextField
                {
                    Label = "Disabled",
                    Default = "Can't change this",
                    Disabled = true,
                },
                new TextField
                {
                    Label = "Will be returned",
                    TextEntered = t => text = t,
                },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : text;
        }

        static string ShowWindowDemo()
        {
            bool canceled = false;

            var script = new List<IPashuaControl>
            {
                new Window
                {
                    Title = "Window Demo",
                    Transparency = 0.85,
                    Floating = true,
                    AutoCloseTime = TimeSpan.FromSeconds(10),
                },
                new Text { Default = "This page will close after 10 seconds" },
                new DefaultButton { Label = "Return to Demo List" },
                new CancelButton { Label = "Quit", Clicked = () => canceled = true },
            };

            script.RunScript();

            return canceled ? null : string.Empty;
        }
    }
}