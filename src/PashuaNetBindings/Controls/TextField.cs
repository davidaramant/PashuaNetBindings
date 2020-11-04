using System;

namespace Pashua
{
    public sealed partial class TextField : IHaveResults
    {
        /// <summary>
        /// Called when the script completes.  The argument will the be the text the user entered (which may be empty).
        /// </summary>
        public Action<string> TextEntered { get; set; }

        void IHaveResults.SetResult(string result) => TextEntered?.Invoke(result);
    }
}