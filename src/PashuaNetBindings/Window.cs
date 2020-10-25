using System;
using System.Collections.Generic;

namespace Pashua
{
    public sealed partial class Window
    {
        partial void FindErrors(List<string> validationErrors)
        {
            if (AutoCloseTime < TimeSpan.Zero)
            {
                validationErrors.Add("Window AutoCloseTime must be positive.");
            }

            if (Transparency < 0 || Transparency > 1)
            {
                validationErrors.Add("Window Transparency must be between 0 and 1.");
            }

            if (X < 0)
            {
                validationErrors.Add("Window X must be greater than 0.");
            }

            if (Y < 0)
            {
                validationErrors.Add("Window Y must be greater than 0.");
            }
        }
    }
}
