using System;

namespace Pashua
{
    public sealed partial class ComboBox : IHaveResults
    {
        private static string SerializeEnum(AutoCompletionMode mode) => ((int) mode).ToString();

        /// <summary>
        /// Called when the script has completed.  The argument is the option chosen by the user.
        /// </summary>
        public Action<string> OptionSelected { get; set; }

        void IHaveResults.SetResult(string result) => OptionSelected?.Invoke(result);
    }
}
