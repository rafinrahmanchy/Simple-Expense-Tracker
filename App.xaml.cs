using System.Windows;
using ExpenseTracker.Services;

namespace ExpenseTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ConfigurationService? _configService;

        /// <summary>
        /// Handles the startup event of the application.
        /// Initializes configuration and creates the main window.
        /// </summary>
        /// <param name="e">The startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize configuration service
            _configService = new ConfigurationService();
            _configService.LoadConfiguration();

            // Create and show the main window
            MainWindow window = new MainWindow();
            window.Show();
        }

        /// <summary>
        /// Gets the configuration service instance.
        /// </summary>
        /// <returns>The configuration service.</returns>
        public ConfigurationService? GetConfigurationService() => _configService;
    }
}
