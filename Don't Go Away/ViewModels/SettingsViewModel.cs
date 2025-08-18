using Core_Logic.Config;
using System.ComponentModel;

namespace Dont_Go_Away.ViewModels
{
    /// <summary>
    /// ViewModel for application settings, providing properties for drift configuration
    /// and change notification for data binding.
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the underlying drift configuration.
        /// </summary>
        public DriftConfig Config { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="config">The drift configuration to bind to.</param>
        public SettingsViewModel(DriftConfig config)
        {
            Config = config;
        }

        /// <summary>
        /// Gets or sets the idle time in milliseconds before drifting starts.
        /// </summary>
        public int IdleThresholdMs
        {
            get => Config.IdleThresholdMs;
            set { Config.IdleThresholdMs = value; OnPropertyChanged(nameof(IdleThresholdMs)); }
        }

        /// <summary>
        /// Gets or sets the size of the drift box in pixels.
        /// </summary>
        public int DriftBoxSize
        {
            get => Config.DriftBoxSize;
            set { Config.DriftBoxSize = value; OnPropertyChanged(nameof(DriftBoxSize)); }
        }

        /// <summary>
        /// Gets or sets the delay between drift steps in milliseconds.
        /// </summary>
        public int StepDelayMs
        {
            get => Config.StepDelayMs;
            set { Config.StepDelayMs = value; OnPropertyChanged(nameof(StepDelayMs)); }
        }

        /// <summary>
        /// Gets or sets the maximum step size in pixels.
        /// </summary>
        public int MaxStep
        {
            get => Config.MaxStep;
            set { Config.MaxStep = value; OnPropertyChanged(nameof(MaxStep)); }
        }

        /// <summary>
        /// Gets or sets the key to simulate during drifting.
        /// </summary>
        public string SimulatedKey
        {
            get => Config.SimulatedKey;
            set { Config.SimulatedKey = value; OnPropertyChanged(nameof(SimulatedKey)); }
        }

        /// <summary>
        /// Gets or sets the chance to simulate a key tap per step (0.0-1.0).
        /// </summary>
        public double ShiftTapChance
        {
            get => Config.ShiftTapChance;
            set { Config.ShiftTapChance = value; OnPropertyChanged(nameof(ShiftTapChance)); }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}