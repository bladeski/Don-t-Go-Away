using Dont_Go_Away.Config;
using Dont_Go_Away.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;

namespace Dont_Go_Away.Views
{
    /// <summary>
    /// Represents the settings page for configuring drift service options.
    /// Loads configuration, binds to the view model, and handles saving changes.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsPage()
        {
            this.InitializeComponent();

            var configPath = Path.Combine(AppContext.BaseDirectory, "config.json");
            var config = ConfigLoader.Load<DriftConfig>(configPath);
            _viewModel = new SettingsViewModel(config);
            this.DataContext = _viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigLoader.Save(_viewModel.Config, "config.json");
        }
    }
}