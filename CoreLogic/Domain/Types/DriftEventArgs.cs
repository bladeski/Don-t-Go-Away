namespace Core_Logic.Domain.Types
{
    /// <summary>
    /// Provides data for the drift state change event.
    /// </summary>
    public class DriftEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether drifting is active.
        /// </summary>
        public bool IsDrifting { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriftEventArgs"/> class.
        /// </summary>
        /// <param name="isDrifting">Whether drifting is active.</param>
        public DriftEventArgs(bool isDrifting)
        {
            IsDrifting = isDrifting;
        }
    }
}