using Core_Logic.Application.Services;
using Core_Logic.Domain.Interfaces;
using Core_Logic.Domain.Interfaces.Services;
using Dont_Go_Away.Services;
using Dont_Go_Away.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;

namespace Dont_Go_Away{
    public partial class App : Application
    {
        public static Window? MainWindowInstance { get; private set; }

        private static readonly bool IsNewInstance;
        private static readonly Mutex AppMutex;

        private readonly IHost _host;
        private readonly ILogger _logger;

        static App()
        {
            AppMutex = new Mutex(true, "Don_t_Go_Away_UniqueAppMutex", out IsNewInstance);
        }

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();

            _logger = _host.Services.GetRequiredService<ILogger>();

            _logger.Log("App constructor called.");
            this.InitializeComponent();

            if (!IsNewInstance)
            {
                _logger.LogError("Another instance is already running. Exiting.");
                Environment.Exit(0);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IConfigLoader, ConfigLoader>();
            services.AddSingleton<IDriftService, DriftService>();
            services.AddSingleton<INavigationService<Frame>, NavigationService<Frame>>();
            services.AddTransient<MainWindow>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _logger.Log("OnLaunched called.");

            try
            {
                _logger.Log("Resolving MainWindow from DI.");
                MainWindowInstance = _host.Services.GetRequiredService<MainWindow>();
                MainWindowInstance.Activate();
                _logger.Log("MainWindow activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during application launch.", ex);
                throw;
            }
        }
    }
}