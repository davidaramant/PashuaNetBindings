using System;

namespace Pashua
{
    public sealed partial class CancelButton : IHaveResults
    {
        /// <summary>
        /// Occurs when the script results indicates that the button was clicked.
        /// </summary>
        public Action Clicked { get; set; }

        void IHaveResults.SetResult(string result)
        {
            var wasClicked = result == "1";
            if (wasClicked)
            {
                Clicked?.Invoke();
            }
        }
    }
}
