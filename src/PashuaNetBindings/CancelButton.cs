namespace Pashua
{
    public sealed partial class CancelButton : IHaveResults
    {
        /// <summary>
        /// Indicates that the user clicked the button. Set after the script is completed.
        /// </summary>
        public bool WasClicked { get; private set; }
        
        void IHaveResults.SetResult(string result)
        {
            WasClicked = result == "1";
        }
    }
}
