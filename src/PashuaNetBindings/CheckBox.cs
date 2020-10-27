namespace Pashua
{
    public sealed partial class CheckBox : IHaveResults
    {
        /// <summary>
        /// Indicates that the user checked the checkbox. Set after the script is completed.
        /// </summary>
        public bool WasChecked { get; private set; }
        
        void IHaveResults.SetResult(string result)
        {
            WasChecked = result == "1";
        }
    }
}