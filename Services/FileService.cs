using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    /// <summary>
    /// Service for handling file I/O operations for expense data persistence.
    /// Uses JSON serialization with modern .NET 10 features for robust storage and retrieval.
    /// </summary>
    public class FileService
    {
        private readonly string _filePath;
        private const string DefaultFileName = "expenses.json";
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the FileService class.
        /// </summary>
        /// <param name="fileName">Optional filename for the data store. Defaults to "expenses.json"</param>
        /// <exception cref="ArgumentException">Thrown when fileName is null or empty.</exception>
        public FileService(string fileName = DefaultFileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        /// <summary>
        /// Saves a collection of expenses to the JSON file with automatic backup.
        /// </summary>
        /// <param name="expenses">The collection of expenses to save.</param>
        /// <exception cref="ArgumentNullException">Thrown when expenses is null.</exception>
        /// <exception cref="IOException">Thrown when file I/O operation fails.</exception>
        public void SaveData(IEnumerable<Expense> expenses)
        {
            ArgumentNullException.ThrowIfNull(expenses);

            try
            {
                // Create backup before saving
                CreateBackupIfFileExists();

                var json = JsonSerializer.Serialize(expenses, _jsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new IOException($"Access denied when saving to file: {_filePath}", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new IOException($"Directory not found for file: {_filePath}", ex);
            }
            catch (JsonException ex)
            {
                throw new IOException($"Error serializing expenses data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error saving data to file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads expenses from the JSON file with error recovery.
        /// </summary>
        /// <returns>A list of expenses, or an empty list if the file doesn't exist.</returns>
        /// <exception cref="IOException">Thrown when file I/O operation fails critically.</exception>
        public List<Expense> LoadData()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Expense>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                
                if (string.IsNullOrWhiteSpace(json))
                    return new List<Expense>();

                return JsonSerializer.Deserialize<List<Expense>>(json, _jsonOptions) ?? new List<Expense>();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new IOException($"Access denied when reading file: {_filePath}", ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new IOException($"File not found: {_filePath}", ex);
            }
            catch (JsonException ex)
            {
                // Try to recover from backup if available
                var backupPath = GetBackupPath();
                if (File.Exists(backupPath))
                {
                    try
                    {
                        var backupJson = File.ReadAllText(backupPath);
                        return JsonSerializer.Deserialize<List<Expense>>(backupJson, _jsonOptions) ?? new List<Expense>();
                    }
                    catch
                    {
                        throw new IOException($"Error deserializing expenses data. File may be corrupted: {ex.Message}", ex);
                    }
                }
                throw new IOException($"Error deserializing expenses data. File may be corrupted: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error loading data from file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the full file path for the data store.
        /// </summary>
        /// <returns>The complete file path.</returns>
        public string GetFilePath() => _filePath;

        /// <summary>
        /// Checks if the data file exists.
        /// </summary>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public bool FileExists() => File.Exists(_filePath);

        /// <summary>
        /// Creates a backup of the current data file.
        /// </summary>
        /// <returns>True if backup was created; otherwise, false.</returns>
        public bool CreateBackup()
        {
            if (!File.Exists(_filePath))
                return false;

            try
            {
                var backupPath = GetBackupPath();
                File.Copy(_filePath, backupPath, overwrite: true);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create backup: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Restores data from the most recent backup.
        /// </summary>
        /// <returns>The list of expenses from the backup, or null if no backup exists.</returns>
        public List<Expense>? RestoreFromBackup()
        {
            var backupPath = GetBackupPath();
            if (!File.Exists(backupPath))
                return null;

            try
            {
                var json = File.ReadAllText(backupPath);
                return JsonSerializer.Deserialize<List<Expense>>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to restore from backup: {ex.Message}");
                return null;
            }
        }

        private void CreateBackupIfFileExists()
        {
            if (File.Exists(_filePath))
            {
                CreateBackup();
            }
        }

        private string GetBackupPath()
        {
            var dir = Path.GetDirectoryName(_filePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            var name = Path.GetFileNameWithoutExtension(_filePath);
            var ext = Path.GetExtension(_filePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(dir, $"{name}.backup.{timestamp}{ext}");
        }
    }
}
