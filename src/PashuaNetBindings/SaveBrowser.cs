namespace Pashua
{
    public sealed partial class SaveBrowser : IHaveResults
    {
        /// <summary>
        /// The path the user selected. Set after the script is completed.
        /// </summary>
        public string SelectedPath { get; private set; }

        void IHaveResults.SetResult(string result)
        {
            SelectedPath = result;
        }
    }
}
