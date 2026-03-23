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
    /// Uses JSON serialization to store and retrieve expense records.
    /// </summary>
    public class FileService
    {
        private readonly string _filePath;
        private const string DefaultFileName = "expenses.json";

        /// <summary>
        /// Initializes a new instance of the FileService class.
        /// </summary>
        /// <param name="fileName">Optional filename for the data store. Defaults to "expenses.json"</param>
        public FileService(string fileName = DefaultFileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            // Save to the application directory
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        /// <summary>
        /// Saves a collection of expenses to the JSON file.
        /// </summary>
        /// <param name="expenses">The collection of expenses to save.</param>
        /// <exception cref="ArgumentNullException">Thrown when expenses is null.</exception>
        /// <exception cref="IOException">Thrown when file I/O operation fails.</exception>
        public void SaveData(IEnumerable<Expense> expenses)
        {
            if (expenses == null)
                throw new ArgumentNullException(nameof(expenses), "Expenses collection cannot be null.");

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var json = JsonSerializer.Serialize(expenses, options);
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
        /// Loads expenses from the JSON file.
        /// </summary>
        /// <returns>A list of expenses, or an empty list if the file doesn't exist.</returns>
        /// <exception cref="IOException">Thrown when file I/O operation fails.</exception>
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

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<Expense>>(json, options) ?? new List<Expense>();
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
    }
}
