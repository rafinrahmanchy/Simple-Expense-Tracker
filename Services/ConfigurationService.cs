using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Services
{
    /// <summary>
    /// Configuration model for application settings.
    /// </summary>
    public class ApplicationConfig
    {
        /// <summary>
        /// Gets or sets the application metadata.
        /// </summary>
        [JsonPropertyName("application")]
        public ApplicationInfo? Application { get; set; }

        /// <summary>
        /// Gets or sets the data storage configuration.
        /// </summary>
        [JsonPropertyName("dataStorage")]
        public DataStorageConfig? DataStorage { get; set; }

        /// <summary>
        /// Gets or sets the UI configuration.
        /// </summary>
        [JsonPropertyName("ui")]
        public UIConfig? UI { get; set; }

        /// <summary>
        /// Gets or sets the logging configuration.
        /// </summary>
        [JsonPropertyName("logging")]
        public LoggingConfig? Logging { get; set; }
    }

    /// <summary>
    /// Application information settings.
    /// </summary>
    public class ApplicationInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "Expense Tracker";

        [JsonPropertyName("version")]
        public string Version { get; set; } = "2.0.0";

        [JsonPropertyName("environment")]
        public string Environment { get; set; } = "Production";
    }

    /// <summary>
    /// Data storage configuration with adaptive environment support.
    /// </summary>
    public class DataStorageConfig
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; } = "expenses.json";

        [JsonPropertyName("useApplicationDirectory")]
        public bool UseApplicationDirectory { get; set; } = true;

        [JsonPropertyName("createBackups")]
        public bool CreateBackups { get; set; } = true;

        [JsonPropertyName("backupRetentionDays")]
        public int BackupRetentionDays { get; set; } = 30;

        [JsonPropertyName("adaptive")]
        public AdaptiveConfig? Adaptive { get; set; }
    }

    /// <summary>
    /// Adaptive environment configuration.
    /// </summary>
    public class AdaptiveConfig
    {
        [JsonPropertyName("enableAutoPathDetection")]
        public bool EnableAutoPathDetection { get; set; } = true;

        [JsonPropertyName("allowCustomPaths")]
        public bool AllowCustomPaths { get; set; } = false;
    }

    /// <summary>
    /// UI configuration.
    /// </summary>
    public class UIConfig
    {
        [JsonPropertyName("defaultWindowWidth")]
        public int DefaultWindowWidth { get; set; } = 1200;

        [JsonPropertyName("defaultWindowHeight")]
        public int DefaultWindowHeight { get; set; } = 800;

        [JsonPropertyName("defaultFont")]
        public string DefaultFont { get; set; } = "Segoe UI";

        [JsonPropertyName("defaultFontSize")]
        public int DefaultFontSize { get; set; } = 12;
    }

    /// <summary>
    /// Logging configuration.
    /// </summary>
    public class LoggingConfig
    {
        [JsonPropertyName("logLevel")]
        public LogLevelConfig? LogLevel { get; set; }

        [JsonPropertyName("enableFileLogging")]
        public bool EnableFileLogging { get; set; } = false;

        [JsonPropertyName("logFilePath")]
        public string LogFilePath { get; set; } = "logs/expensetracker.log";
    }

    /// <summary>
    /// Log level configuration.
    /// </summary>
    public class LogLevelConfig
    {
        [JsonPropertyName("default")]
        public string Default { get; set; } = "Information";

        [JsonPropertyName("system")]
        public string System { get; set; } = "Warning";

        [JsonPropertyName("microsoft")]
        public string Microsoft { get; set; } = "Warning";
    }

    /// <summary>
    /// Service for managing application configuration with adaptive environment support.
    /// Supports loading configuration from appsettings.json and environment-specific overrides.
    /// </summary>
    public class ConfigurationService
    {
        private ApplicationConfig? _config;
        private readonly string _configPath;
        private const string DefaultConfigFileName = "appsettings.json";

        /// <summary>
        /// Initializes a new instance of the ConfigurationService class.
        /// </summary>
        /// <param name="configFileName">Optional filename for configuration. Defaults to "appsettings.json"</param>
        public ConfigurationService(string configFileName = DefaultConfigFileName)
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
        }

        /// <summary>
        /// Loads the configuration from file.
        /// </summary>
        /// <returns>True if configuration was loaded successfully; otherwise, false.</returns>
        public bool LoadConfiguration()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Configuration file not found at {_configPath}. Using defaults.");
                    _config = new ApplicationConfig();
                    return false;
                }

                var json = File.ReadAllText(_configPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                _config = JsonSerializer.Deserialize<ApplicationConfig>(json, options) ?? new ApplicationConfig();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading configuration: {ex.Message}");
                _config = new ApplicationConfig();
                return false;
            }
        }

        /// <summary>
        /// Gets the current application configuration.
        /// </summary>
        /// <returns>The application configuration, or a default configuration if not loaded.</returns>
        public ApplicationConfig GetConfiguration()
        {
            return _config ?? new ApplicationConfig();
        }

        /// <summary>
        /// Gets the data storage configuration.
        /// </summary>
        /// <returns>The data storage configuration.</returns>
        public DataStorageConfig GetDataStorageConfig()
        {
            return GetConfiguration().DataStorage ?? new DataStorageConfig();
        }

        /// <summary>
        /// Gets the UI configuration.
        /// </summary>
        /// <returns>The UI configuration.</returns>
        public UIConfig GetUIConfig()
        {
            return GetConfiguration().UI ?? new UIConfig();
        }

        /// <summary>
        /// Gets the storage path based on adaptive environment settings.
        /// </summary>
        /// <returns>The resolved storage path.</returns>
        public string GetStoragePath()
        {
            var dataConfig = GetDataStorageConfig();
            if (dataConfig.Adaptive?.EnableAutoPathDetection ?? true)
            {
                return ResolveAdaptivePath();
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Resolves the storage path based on the current environment.
        /// </summary>
        /// <returns>The resolved adaptive storage path.</returns>
        private string ResolveAdaptivePath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = GetConfiguration().Application?.Name ?? "ExpenseTracker";
            var storagePath = Path.Combine(appDataPath, appName);

            try
            {
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                }
                return storagePath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating adaptive storage path: {ex.Message}. Falling back to application directory.");
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
