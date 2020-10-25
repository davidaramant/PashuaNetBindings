using System;
using System.Collections.Generic;

namespace Pashua
{
    public sealed partial class Window
    {
        public override IEnumerable<string> GetValidationIssues()
        {
            if (AutoCloseTime < TimeSpan.Zero)
            {
                yield return "Window AutoCloseTime must be positive.";
            }

            if (Transparency < 0 || Transparency > 1)
            {
                yield return "Window Transparency must be between 0 and 1.";
            }

            if (X < 0)
            {
                yield return "Window X must be greater than 0.";
            }

            if (Y < 0)
            {
                yield return "Window Y must be greater than 0.";
            }
        }
    }
}
