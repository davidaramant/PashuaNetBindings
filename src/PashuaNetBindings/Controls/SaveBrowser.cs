using System;

namespace Pashua
{
    public sealed partial class SaveBrowser : IHaveResults
    {
        /// <summary>
        /// Called when the script has completed.  The argument is the path the user chose (may be empty).
        /// </summary>
        public Action<string> PathSelected { get; set; }

        void IHaveResults.SetResult(string result) => PathSelected?.Invoke(result);
    }
}
