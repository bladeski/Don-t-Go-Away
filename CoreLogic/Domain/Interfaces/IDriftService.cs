using Core_Logic.Domain.Types;

namespace Core_Logic.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a drift service that simulates user activity to prevent system idle.
    /// </summary>
    public interface IDriftService
    {
        /// <summary>
        /// Occurs when the drift state changes (started or stopped).
        /// </summary>
        event EventHandler<DriftEventArgs>? DriftStateChanged;

        /// <summary>
        /// Gets whether the drift service is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the drift service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the drift service.
        /// </summary>
        void Stop();

        /// <summary>
        /// Toggles the drift service between running and stopped states.
        /// </summary>
        void Toggle();
    }
}