using System;

namespace Pashua
{
    public sealed partial class CheckBox : IHaveResults
    {
        /// <summary>
        /// Occurs when the script results indicates that the checkbox was clicked.
        /// </summary>
        public Action Checked { get; set; }

        void IHaveResults.SetResult(string result)
        {
            var wasChecked = result == "1";
            if (wasChecked)
            {
                Checked?.Invoke();
            }
        }
    }
}