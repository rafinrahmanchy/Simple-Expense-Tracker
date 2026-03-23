using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModels
{
    /// <summary>
    /// Defines an asynchronous command that can be used in MVVM bindings.
    /// </summary>
    public interface IAsyncRelayCommand : ICommand
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(object? parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event to notify that the command's execution state may have changed.
        /// </summary>
        void RaiseCanExecuteChanged();
    }

    /// <summary>
    /// An asynchronous relay command implementation for MVVM patterns with support for can-execute predicates.
    /// </summary>
    public class AsyncRelayCommand : IAsyncRelayCommand
    {
        private readonly Func<object?, Task> _executeAsync;
        private readonly Predicate<object?>? _canExecute;
        private bool _isExecuting;

        /// <summary>
        /// Initializes a new instance of the AsyncRelayCommand class.
        /// </summary>
        /// <param name="executeAsync">The async action to execute when the command is invoked.</param>
        /// <param name="canExecute">Optional predicate to determine if the command can be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when executeAsync is null.</exception>
        public AsyncRelayCommand(Func<object?, Task> executeAsync, Predicate<object?>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when the execution state of the command changes.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can be executed and is not currently executing; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter));
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public async void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await ExecuteAsync(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ExecuteAsync(object? parameter)
        {
            if (!CanExecute(parameter))
                return Task.CompletedTask;

            _isExecuting = true;
            RaiseCanExecuteChanged();

            return _executeAsync(parameter).ContinueWith(task =>
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            });
        }

        /// <summary>
        /// Raises the CanExecuteChanged event to notify that the command's execution state may have changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
