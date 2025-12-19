using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels
{
    /// <summary>
    /// Main view model for the Expense Tracker application.
    /// Manages the state of expenses, handles user interactions, and coordinates with the FileService.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FileService _fileService;
        private Expense? _selectedExpense;
        private string _descriptionInput = string.Empty;
        private decimal _amountInput;
        private DateTime _dateInput = DateTime.Now;
        private string _monthSummary = string.Empty;
        private bool _isUpdatingSelectedExpense = false;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the observable collection of all expenses.
        /// </summary>
        public ObservableCollection<Expense> Expenses { get; set; } = new ObservableCollection<Expense>();

        /// <summary>
        /// Gets or sets the currently selected expense from the list.
        /// Populates input fields when an expense is selected.
        /// </summary>
        public Expense? SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                if (_isUpdatingSelectedExpense) return; // Prevent recursive updates
                if (_selectedExpense == value) return; // Prevent unnecessary updates

                _isUpdatingSelectedExpense = true;
                try
                {
                    _selectedExpense = value;
                    OnPropertyChanged();
                    
                    // Populate inputs when selection changes
                    if (_selectedExpense != null)
                    {
                        DescriptionInput = _selectedExpense.Description;
                        AmountInput = _selectedExpense.Amount;
                        DateInput = _selectedExpense.Date;
                    }
                }
                finally
                {
                    _isUpdatingSelectedExpense = false;
                }
                
                // Force command re-evaluation
                if (UpdateCommand is RelayCommand updateCmd)
                    updateCmd.RaiseCanExecuteChanged();
                if (DeleteCommand is RelayCommand deleteCmd)
                    deleteCmd.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description input for adding/editing expenses.
        /// </summary>
        public string DescriptionInput
        {
            get => _descriptionInput;
            set { _descriptionInput = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the amount input for adding/editing expenses.
        /// </summary>
        public decimal AmountInput
        {
            get => _amountInput;
            set { _amountInput = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the date input for adding/editing expenses.
        /// </summary>
        public DateTime DateInput
        {
            get => _dateInput;
            set { _dateInput = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the monthly summary text.
        /// </summary>
        public string MonthSummary
        {
            get => _monthSummary;
            set { _monthSummary = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the command for adding a new expense.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Gets the command for updating the selected expense.
        /// </summary>
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// Gets the command for deleting the selected expense.
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets the command for clearing the selection and resetting the form.
        /// </summary>
        public ICommand ClearSelectionCommand { get; }

        /// <summary>
        /// Gets the command for calculating the monthly summary.
        /// </summary>
        public ICommand CalculateSummaryCommand { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// Initializes commands and loads initial data from storage.
        /// </summary>
        public MainViewModel()
        {
            _fileService = new FileService();
            
            // Initialize commands
            AddCommand = new RelayCommand(AddExpense);
            UpdateCommand = new RelayCommand(UpdateExpense, CanUpdateOrDelete);
            DeleteCommand = new RelayCommand(DeleteExpense, CanUpdateOrDelete);
            ClearSelectionCommand = new RelayCommand(ClearSelection);
            CalculateSummaryCommand = new RelayCommand(CalculateSummary);

            // Load initial data
            LoadExpenses();
        }

        private void LoadExpenses()
        {
            var data = _fileService.LoadData();
            Expenses.Clear();
            foreach (var item in data)
            {
                Expenses.Add(item);
            }
            CalculateSummary(null);
        }

        private void SaveExpenses()
        {
            try
            {
                _fileService.SaveData(Expenses);
                CalculateSummary(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving expenses: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddExpense(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(DescriptionInput))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AmountInput <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newExpense = new Expense
            {
                Description = DescriptionInput,
                Amount = AmountInput,
                Date = DateInput
            };

            Expenses.Add(newExpense);
            SaveExpenses();
            ClearInputs();
            ClearSelection(null);
        }

        private void UpdateExpense(object? parameter)
        {
            if (SelectedExpense == null) return;

            if (string.IsNullOrWhiteSpace(DescriptionInput))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AmountInput <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the expense object directly
            SelectedExpense.Description = DescriptionInput;
            SelectedExpense.Amount = AmountInput;
            SelectedExpense.Date = DateInput;

            // Refresh the collection to trigger UI update
            int index = Expenses.IndexOf(SelectedExpense);
            if (index != -1)
            {
                Expenses[index] = SelectedExpense;
            }

            SaveExpenses();
            ClearInputs();
            SelectedExpense = null;
            MessageBox.Show("Expense updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanUpdateOrDelete(object? parameter)
        {
            return SelectedExpense != null;
        }

        private void DeleteExpense(object? parameter)
        {
            if (SelectedExpense == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedExpense.Description}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Expenses.Remove(SelectedExpense);
                SaveExpenses();
                ClearInputs();
                SelectedExpense = null;
                MessageBox.Show("Expense deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearSelection(object? parameter)
        {
            SelectedExpense = null;
            ClearInputs();
        }

        private void ClearInputs()
        {
            DescriptionInput = string.Empty;
            AmountInput = 0;
            DateInput = DateTime.Now;
        }

        private void CalculateSummary(object? parameter)
        {
            // Group by Month/Year
            var grouped = Expenses.GroupBy(e => new { e.Date.Year, e.Date.Month })
                                  .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month);

            var summaryLines = grouped.Select(g => $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMMM yyyy}: {g.Sum(e => e.Amount):C}");
            MonthSummary = string.Join(Environment.NewLine, summaryLines);
            
            if (string.IsNullOrEmpty(MonthSummary))
            {
                MonthSummary = "No expenses recorded.";
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
