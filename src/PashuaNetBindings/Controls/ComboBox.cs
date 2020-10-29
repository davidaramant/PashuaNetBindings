namespace Pashua
{
    public sealed partial class ComboBox : IHaveResults
    {
        private static string SerializeEnum(AutoCompletionMode mode) => ((int) mode).ToString();

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
