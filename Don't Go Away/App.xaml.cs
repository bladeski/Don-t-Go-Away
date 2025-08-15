using Dont_Go_Away.Helpers;
using Dont_Go_Away.Views;
using Microsoft.UI.Xaml;
using System;
using System.Threading;

namespace Dont_Go_Away
{
    /// <summary>
    /// The main application class for Don't Go Away.
    /// Handles single instance enforcement, startup, and main window activation.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the main window instance for the application.
        /// </summary>
        public static Window? MainWindowInstance { get; private set; }

        /// <summary>
        /// Indicates whether this is the first instance of the application.
        /// </summary>
        private static readonly bool IsNewInstance;

        /// <summary>
        /// The mutex used to enforce single instance behavior.
        /// </summary>
        private static readonly Mutex AppMutex;

        /// <summary>
        /// Static constructor. Initializes the application mutex and instance flag.
        /// </summary>
        static App()
        {
            AppMutex = new Mutex(true, "Don_t_Go_Away_UniqueAppMutex", out IsNewInstance);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Logs construction and exits if another instance is running.
        /// </summary>
        public App()
        {
            Logger.Log("App constructor called.");
            this.InitializeComponent();

            if (!IsNewInstance)
            {
                Logger.LogError("Another instance is already running. Exiting.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Handles application launch, creates and activates the main window.
        /// </summary>
        /// <param name="args">Launch arguments.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Logger.Log("OnLaunched called.");

            try
            {
                Logger.Log("Creating MainWindow.");
                MainWindowInstance = new MainWindow();
                MainWindowInstance.Activate();
                Logger.Log("MainWindow activated.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error during application launch.", ex);
                throw;
            }
        }
    }
}