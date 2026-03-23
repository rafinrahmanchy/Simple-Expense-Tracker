using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExpenseTracker.Models
{
    /// <summary>
    /// Represents a single expense entry in the Expense Tracker application.
    /// Implements INotifyPropertyChanged to support data binding in the UI.
    /// </summary>
    public class Expense : INotifyPropertyChanged
    {
        private Guid _id = Guid.NewGuid();
        private string _description = string.Empty;
        private decimal _amount;
        private DateTime _date = DateTime.Now;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the unique identifier for this expense.
        /// </summary>
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the description of the expense (e.g., "Groceries", "Gas").
        /// </summary>
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the amount spent in this expense.
        /// </summary>
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the date when the expense occurred.
        /// </summary>
        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets a formatted string representation of the expense date.
        /// </summary>
        public string DisplayDate => Date.ToShortDateString();

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
