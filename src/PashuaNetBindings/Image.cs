using System.Collections.Generic;

namespace Pashua
{
    public sealed partial class Image
    {
        partial void AdditionalValidation(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                errors.Add("Image Path must be specified.");
            }

            var dimensionSpecified = Width != null || Height != null;
            var maxDimensionSpecified = MaxWidth != null || MaxHeight != null;
            if (dimensionSpecified && maxDimensionSpecified)
            {
                errors.Add("Image Width/Height cannot be specified if MaxWidth/MaxHeight is specified.");
            }
        }
    }
}
