using System;
using System.Collections.Generic;
using Pashua.Backend;

namespace Pashua
{
    public static class PashuaDialog
    {
        public static DialogBuilder Create(
            string title = null,
            double? transparency = null,
            bool brushedMetal = false,
            TimeSpan? autoCloseTime = null,
            bool? floating = false,
            int? x = null,
            int? y = null)
        {
            var script = new List<string>();
            var context = new ControlContext(script);
            context.Set("title", title);
            context.Set("transparency", transparency);
            context.Set("autoclosetime", (int?)autoCloseTime?.TotalSeconds);
            context.Set("floating", floating, false);
            context.Set("x", x);
            context.Set("y", y);

            if (brushedMetal) script.Add("*.appearance = metal");

            return new DialogBuilder(script);
        }
    }
}