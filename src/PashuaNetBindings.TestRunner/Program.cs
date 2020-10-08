using System;
using Pashua;

namespace PashuaNetBindings.TestRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ShowUsingFluentInterface();
        }

        private static void ShowUsingFluentInterface()
        {
            PashuaDialog.Create("Hello", floating: true, brushedMetal: true).WithButton("one", "Another option")
                .WithDefaultButton(label: "Yup").WithCancelButton("cancel", "Cancel button")
                .WithCheckBox("check", "Checkbox", tooltip: "Yup").WithComboBox("combo",
                    "Here are some options",
                    new[] {"Cat", "Dog", "Horse"},
                    "Dog",
                    AutoCompletion.CaseInsensitive)
                .WithDate("Date1", chooseTime: true, defaultDateTime: new DateTime(2013, 01, 01, 01, 02, 03))
                .WithImage("test.png", label: "Test image", border: true, tooltip: "Sure is").WithOpenBrowser("browser",
                    "Select an image",
                    defaultPath: "test.png",
                    fileTypes: new[] {"jpg", "gif", "png"},
                    width: 350).WithPassword("password", "Enter a password", defaultText: "secret", width: 120)
                .WithPopup("popup", label: "Pick a number:", options: new[] {"One", "Two", "Three"},
                    defaultValue: "Two").WithRadioButtons("radiobuttons",
                    label: "Pick a number:",
                    options: new[] {"One", "Two", "Three"},
                    defaultValue: "Two").WithSaveBrowser("save", "Set save path:", fileType: "png")
                .WithText("Here is some text")
                .WithTextBox("box", defaultText: "First line\nSecond line", monospaceFont: true)
                .WithTextField("textField", "Enter a string", defaultText: "secret", width: 120).Show();
        }
    }
}