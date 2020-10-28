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
        }

        static Page? ShowDemoSelection(string lastOutput)
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Control Demos" },
                new Text { Default = "This page also serves as the RadioButton demo" }
            };
            var option = script.AddAndReturn(
                new RadioButton
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

            var boxesChecked = new List<string>();

            if (defaultCb.WasChecked)
            {
                boxesChecked.Add("Default");
            }

            if (withTooltip.WasChecked)
            {
                boxesChecked.Add("Tooltip");
            }

            return "Boxes checked: " + string.Join(", ", boxesChecked);
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
                Options = new[] { "A", "B", "C" },
                Completion = AutoCompletionMode.None,
                Placeholder = "Placeholder text",
                Width = 500,
            });
            var caseInsensitive = script.AddAndReturn(new ComboBox
            {
                Label = "Case-Insensitive Completion",
                Options = new[] { "A", "B", "C" },
                Completion = AutoCompletionMode.CaseSensitive,
                Rows = 4,
            });

            script.Run();

            if (cancel.WasClicked)
                return null;

            return noCompletion.SelectedOption;
        }

        static string ShowDateDemos()
        {
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
                }
            };
            var both = script.AddAndReturn(new Date
            {
                Label = "Date & Time - Will be returned",
                SelectionMode = DateTimeSelection.BothTimeAndDate,

            });
            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            if (cancel.WasClicked)
                return null;

            return both.SelectedTimestamp.ToString("s");
        }

        static string ShowImageDemos()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Image Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Image
                {
                    Label = "Default Options",
                    Path = "test.png",
                },
                new Image
                {
                    Label = "Border & Size Adjustment",
                    Path = "test.png",
                    Border = true,
                    Width = 160,
                    Height = 90,
                    Upscale = true,
                },
                new Image
                {
                    Label = "Max Dimensions (bug in Pashua?)",
                    Path = "test.png",
                    MaxWidth = 10,
                }
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : "";
        }

        static string ShowOpenBrowserDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new OpenBrowser
            {
                Label = "Will be returned",
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.SelectedPath;
        }

        static string ShowPasswordDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new Password
            {
                Label = "Will be returned",
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.EnteredText;
        }

        static string ShowPopupDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new Popup
            {
                Label = "Will be returned",
                Options = new[] { "A", "B", "C" }
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.SelectedOption;
        }

        static string ShowSaveBrowserDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new SaveBrowser
            {
                Label = "Will be returned",
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.SelectedPath;
        }

        static string ShowTextDemos()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Text Demos"},
                new DefaultButton { Label = "Return to Demo List" },
                new Text
                {
                    Label = "This is a label, which is different than the main text",
                    Default = "The text\ncan\nhave\nnewlines",
                },
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : string.Empty;
        }

        static string ShowTextBoxDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new TextBox
            {
                Label = "Will be returned",
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.EnteredText;
        }

        static string ShowTextFieldDemos()
        {
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
            };
            var realValue = script.AddAndReturn(new TextField
            {
                Label = "Will be returned",
            });

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return cancel.WasClicked ? null : realValue.EnteredText;
        }
    }
}