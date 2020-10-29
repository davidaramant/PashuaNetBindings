namespace Pashua
{
    public sealed partial class TextField : IHaveResults
    {
        /// <summary>
        /// The text the user entered. Set after the script is completed.
        /// </summary>
        public string EnteredText { get; private set; }

        void IHaveResults.SetResult(string result)
        {
            EnteredText = result;
        }
    }
}