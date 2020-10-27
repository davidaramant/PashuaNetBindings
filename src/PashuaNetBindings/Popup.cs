namespace Pashua
{
    public sealed partial class Popup : IHaveResults
    {
        /// <summary>
        /// The option the user selected. Set after the script is completed.
        /// </summary>
        public string SelectedOption { get; private set; }
        
        void IHaveResults.SetResult(string result)
        {
            SelectedOption = result;
        }
    }
}
