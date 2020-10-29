using System;
using System.Collections.Generic;

namespace Pashua
{
    public sealed partial class Window
    {
        partial void AdditionalValidation(List<string> errors)
        {
            if (AutoCloseTime < TimeSpan.Zero)
            {
                errors.Add("Window AutoCloseTime must be positive.");
            }

            if (Transparency < 0 || Transparency > 1)
            {
                errors.Add("Window Transparency must be between 0 and 1.");
            }
        }
    }
}
