using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pashua
{
    public sealed partial class OpenBrowser: IHaveResults
    {
        /// <summary>
        /// File types that can be selected in the open dialog or dropped onto the textfield. In addition to filename
        /// extensions, you can use "directory" to let the user choose directories.
        /// </summary>
        /// <remarks>
        /// If only "directory" is specified, the user won’t be able to choose any files. If only filename extensions are
        /// specified, the user won’t be able to choose directories. If you don’t specify filetype at all, the user
        /// will be able to choose anything in the open dialog box.
        /// </remarks>
        public IEnumerable<string> FileTypes { get; set; }
        
        /// <summary>
        /// The path the user selected. Set after the script is completed.
        /// </summary>
        public string SelectedPath { get; private set; }
        
        void IHaveResults.SetResult(string result)
        {
            SelectedPath = result;
        }

        private bool IsFileTypesSpecified => FileTypes?.Any() ?? false;

        partial void WriteSpecialProperties(StreamWriter writer)
        {
            if (IsFileTypesSpecified)
            {
                writer.WriteLine($"{Id}.filetype = {string.Join(" ", FileTypes)}");
            }
        }

        partial void AdditionalValidation(List<string> errors)
        {
            if(IsFileTypesSpecified && FileTypes.Any(string.IsNullOrWhiteSpace))
            {
                errors.Add("OpenBrowser FileTypes cannot contain null or empty strings.");
            }
        }
    }
}
