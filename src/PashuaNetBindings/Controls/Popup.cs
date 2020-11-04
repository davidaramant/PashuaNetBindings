using System;

namespace Pashua
{
    public sealed partial class Popup : IHaveResults
    {
        /// <summary>
        /// Called when the script has completed.  The argument is the option chosen by the user.
        /// </summary>
        public Action<string> OptionSelected { get; set; }

        void IHaveResults.SetResult(string result) => OptionSelected?.Invoke(result);
    }
}
