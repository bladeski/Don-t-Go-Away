namespace Dont_Go_Away.Config
{
    /// <summary>
    /// Represents configuration options for the drift service.
    /// </summary>
    public class DriftConfig
    {
        /// <summary>Idle time in milliseconds before drifting starts.</summary>
        public int IdleThresholdMs { get; set; } = 120000;
        /// <summary>Size of the drift box in pixels.</summary>
        public int DriftBoxSize { get; set; } = 500;
        /// <summary>Delay between drift steps in milliseconds.</summary>
        public int StepDelayMs { get; set; } = 50;
        /// <summary>Maximum step size in pixels.</summary>
        public int MaxStep { get; set; } = 2;
        /// <summary>Chance to simulate a key tap per step (0.0-1.0).</summary>
        public double ShiftTapChance { get; set; } = 0.01;
        /// <summary>The key to simulate during drifting.</summary>
        public string SimulatedKey { get; set; } = "SHIFT";
    }
}
