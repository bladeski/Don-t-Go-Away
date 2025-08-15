using Dont_Go_Away.Helpers;
using Dont_Go_Away.Interop;
using Dont_Go_Away.Services;
using Dont_Go_Away.Types;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Drawing;
using System.IO;

namespace Dont_Go_Away.Views
{
    /// <summary>
    /// The main window for the Don't Go Away application.
    /// Handles UI events, tray icon, and drift service lifecycle.
    /// </summary>
    public sealed partial class MainWindow : Window, IDisposable
    {
        private readonly DriftService? _driftService;
        private readonly WinUITrayIcon? _trayIcon;

        private readonly Icon? _activeIcon;
        private readonly Icon? _inactiveIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            NavigationService.Initialize(RootFrame);
            RootFrame.Navigate(typeof(HomePage));
            RootFrame.Navigated += OnNavigated;


            try
            {
                _activeIcon = LoadIcon("AppIcon.ico");
                _inactiveIcon = LoadIcon("AppIcon_Inactive.ico");

                _trayIcon = new WinUITrayIcon(
                    this,
                    _inactiveIcon,
                    "Don't Go Away",
                    () => Activate()
                );

                _driftService = new DriftService();
                _driftService.DriftStateChanged += OnDriftStateChanged;

                WinUIInterop.CenterWindow(this, 640, 480);
            }
            catch (Exception ex)
            {
                Logger.LogError("MainWindow initialization failed", ex);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            BackButton.Visibility = NavigationService.CanGoBack
                ? Visibility.Visible
                : Visibility.Collapsed;
            SettingsButton.Visibility = NavigationService.CanGoBack
                ? Visibility.Collapsed
                : Visibility.Visible;
            RootFrame.Visibility = RootFrame.Content == null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the toggling of the GoAwayToggle control.
        /// Starts or stops the drift service and updates the tray icon.
        /// </summary>
        private void GoAwayToggle_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GoAwayToggle.IsOn)
                {
                    _driftService?.Start();
                    if (RootFrame.Content.GetType() == typeof(SettingsPage))
                    {
                        NavigationService.GoBack();
                    }
                }
                else
                {
                    _driftService?.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Toggle error", ex);
            }
        }

        /// <summary>
        /// Handles the click event for the Settings button.
        /// Navigates to the application's settings page.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(SettingsPage));

            if (App.MainWindowInstance == null)
            {
                Logger.LogError("MainWindowInstance is null, cannot navigate to SettingsPage.");
                return;
            }
        }

        /// <summary>
        /// Handles the drift state change event from the drift service.
        /// Shows a balloon tip in the tray icon.
        /// </summary>
        private void OnDriftStateChanged(object? sender, DriftEventArgs args)
        {
            try
            {
                GoAwayToggle.IsOn = args.IsDrifting;
                string message = args.IsDrifting ? "Service now running." : "Service stopped.";
                _trayIcon?.ShowBalloonTip("Don't Go Away", message);
                _trayIcon?.UpdateIconTooltip(args.IsDrifting ? "Don't Go Away - Running" : "Don't Go Away - Stopped");
                _trayIcon?.UpdateIcon(args.IsDrifting ? _activeIcon : _inactiveIcon);
                SettingsButton.IsEnabled = !args.IsDrifting;
            }
            catch (Exception ex)
            {
                Logger.LogError("DriftStateChanged error", ex);
            }
        }

        /// <summary>
        /// Loads an icon from the Assets folder.
        /// </summary>
        /// <param name="fileName">The icon file name.</param>
        /// <returns>The loaded <see cref="Icon"/>.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the icon file is missing.</exception>
        private static Icon LoadIcon(string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);

            if (!File.Exists(path))
            {
                Logger.LogError($"Icon file not found: {path}");
                throw new FileNotFoundException("Icon file missing", path);
            }

            return new Icon(path);
        }

        /// <summary>
        /// Disposes resources used by the window.
        /// </summary>
        public void Dispose()
        {
            _activeIcon?.Dispose();
            _inactiveIcon?.Dispose();
        }
    }
}