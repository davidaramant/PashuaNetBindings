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
        }
    }
}
