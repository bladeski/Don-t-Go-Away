using Core_Logic.Config;
using Core_Logic.Domain.Interfaces;
using Core_Logic.Domain.Types;
using Core_Logic.Infrastructure.Interop;
using Dont_Go_Away.Interop;
using InputSimulatorEx;
using InputSimulatorEx.Native;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Dont_Go_Away.Services
{
    /// <summary>
    /// Provides functionality to simulate user activity (drifting) to prevent system idle.
    /// </summary>
    public class DriftService : IDriftService
    {

        /// <summary>
        /// Occurs when the drift state changes (started or stopped).
        /// </summary>
        public event EventHandler<DriftEventArgs>? DriftStateChanged;

        private readonly DriftConfig config;
        private readonly InputSimulator sim;
        private readonly Random rand;
        private CancellationTokenSource? _cts;
        private Task? _driftTask;
        private readonly ILogger _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="DriftService"/> class.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Config loading uses reflection")]
        public DriftService(ILogger logger, IConfigLoader configLoader, string configPath = "config.json")
        {
            var config = configLoader.Load<DriftConfig>(configPath);
            this.config = config;
            sim = new InputSimulator();
            rand = new Random();
            _logger = logger;
        }

        public bool IsRunning => _driftTask is { IsCompleted: false };

        public void Toggle()
        {
            if (IsRunning) Stop();
            else Start();
        }

        /// <summary>
        /// Starts the drift service if not already running.
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;
            _cts = new CancellationTokenSource();
            _driftTask = Task.Run(() => RunAsync(_cts.Token), CancellationToken.None);
            OnDriftStateChanged(true);
        }

        /// <summary>
        /// Stops the drift service if running.
        /// </summary>
        public void Stop()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            _driftTask = null;
            OnDriftStateChanged(false);
        }

        /// <summary>
        /// Raises the <see cref="DriftStateChanged"/> event.
        /// </summary>
        /// <param name="isDrifting">Indicates whether drifting is active.</param>
        protected virtual void OnDriftStateChanged(bool isDrifting)
        {
            DriftStateChanged?.Invoke(this, new DriftEventArgs(isDrifting));
        }

        /// <summary>
        /// Main loop for simulating user activity.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        private async Task RunAsync(CancellationToken token)
        {
            int idleThreshold = config.IdleThresholdMs;
            int boxSize = config.DriftBoxSize;
            int stepDelay = config.StepDelayMs;
            int maxStep = config.MaxStep;
            double shiftChance = config.ShiftTapChance;
            VirtualKeyCode simulatedKey = ParseKey(config.SimulatedKey);

            int currentX = 0, currentY = 0;
            int minX = 0, minY = 0, maxX = 0, maxY = 0;
            bool drifting = false;
            double angle = 0;

            // Reduce delay for more responsive idle detection
            int idleCheckInterval = Math.Min(stepDelay, 500);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(idleCheckInterval, token);
                    uint idleTime = GetIdleTime();

                    if (idleTime > idleThreshold)
                    {
                        if (!drifting)
                        {
                            if (User32Interop.GetCursorPos(out POINT origin))
                            {
                                currentX = origin.X;
                                currentY = origin.Y;
                                minX = origin.X - boxSize / 2;
                                minY = origin.Y - boxSize / 2;
                                maxX = origin.X + boxSize / 2;
                                maxY = origin.Y + boxSize / 2;
                                drifting = true;
                                OnDriftStateChanged(true);
                            }
                            else
                            {
                                _logger.LogError("Failed to get cursor position.");
                                continue;
                            }
                        }

                        angle += 0.2;
                        int dx = (int)(Math.Cos(angle) * maxStep);
                        int dy = (int)(Math.Sin(angle) * maxStep);

                        currentX = Math.Clamp(currentX + dx, minX, maxX);
                        currentY = Math.Clamp(currentY + dy, minY, maxY);

                        if (!User32Interop.SetCursorPos(currentX, currentY))
                        {
                            _logger.LogError("Failed to set cursor position.");
                        }

                        if (shiftChance > 0 && rand.NextDouble() < shiftChance)
                        {
                            sim.Keyboard.KeyPress(simulatedKey);
                        }

                        await Task.Delay(stepDelay, token);
                    }
                    else if (drifting)
                    {
                        drifting = false;
                        OnDriftStateChanged(false);
                        // Reset drift state
                        currentX = currentY = minX = minY = maxX = maxY = 0;
                        angle = 0;
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.Log("Drift task cancelled.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Drift loop error", ex);
                }
            }
        }

        /// <summary>
        /// Gets the system idle time in milliseconds.
        /// </summary>
        /// <returns>The idle time in milliseconds.</returns>
        private uint GetIdleTime()
        {
            LASTINPUTINFO lastInput = new()
            {
                cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf<LASTINPUTINFO>()
            };

            if (!User32Interop.GetLastInputInfo(ref lastInput))
            {
                _logger.LogError("GetLastInputInfo failed.");
                return 0;
            }

            return (uint)Environment.TickCount - lastInput.dwTime;
        }

        /// <summary>
        /// Parses a string to a <see cref="VirtualKeyCode"/>.
        /// </summary>
        /// <param name="keyName">The key name as a string.</param>
        /// <returns>The parsed <see cref="VirtualKeyCode"/>.</returns>
        private VirtualKeyCode ParseKey(string keyName)
        {
            try
            {
                return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), keyName.ToUpperInvariant());
            }
            catch
            {
                _logger.LogError($"Invalid key '{keyName}' in config. Defaulting to SHIFT.");
                return VirtualKeyCode.SHIFT;
            }
        }
    }
}