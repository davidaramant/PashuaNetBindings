using System;
using System.Collections.Generic;
using System.Linq;
using Pashua;
using Pashua.ScriptExtensions;

namespace PashuaNetBindings.Demo
{
    internal class Program
    {
        private const bool ShouldContinue = true;

        private static void Main(string[] args)
        {
            Page? page = Page.DemoOverview;

            Page? ShowDemoPage(Func<bool> showPage)
            {
                return showPage() == ShouldContinue ? (Page?)Page.DemoOverview : null;
            }

            while (page.HasValue)
            {
                switch (page)
                {
                    case Page.DemoOverview:
                        page = ShowDemoSelection();
                        break;
                    case Page.Buttons:
                        page = ShowDemoPage(ShowButtons);
                        break;
                }
            }
        }

        enum Page
        {
            DemoOverview,
            Buttons,
        }

        static Page? ShowDemoSelection()
        {
            var pageOptions = ((Page[])Enum.GetValues(typeof(Page))).Where(p => p != Page.DemoOverview)
                .Select(p => p.ToString());

            var script = new List<IPashuaControl>();
            var option = script.AddAndReturn(
                new ComboBox
                {
                    Label = "Select demo page to view:",
                    Options = pageOptions,
                });
            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });
            var ok = script.AddAndReturn(new DefaultButton { Label = "Show Page" });

            script.Run();

            if (cancel.WasClicked)
            {
                return null;
            }
            else
            {
                return Enum.TryParse(typeof(Page), option.SelectedOption, out object page) ? (Page?)page : null;
            }
        }

        static bool ShowButtons()
        {
            var script = new List<IPashuaControl>
            {
                new Window { Title = "Button Demos"},
                new Text { Default = "Various button functionality."},
                new Button {Label = "Has Tooltip", Tooltip = "Multiline\nTooltip"},
                new Button { Label="Disabled", Disabled = true},
            };

            var cancel = script.AddAndReturn(new CancelButton { Label = "Quit" });

            script.Run();

            return !cancel.WasClicked;
        }
    }
}