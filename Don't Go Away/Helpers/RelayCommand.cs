using System;
using System.Windows.Input;

namespace Dont_Go_Away.Helpers
{
    /// <summary>
    /// A simple implementation of <see cref="ICommand"/> for relaying actions.
    /// </summary>
    public partial class RelayCommand(Action execute) : ICommand
    {
        private readonly Action _execute = execute;

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc/>
        public void Execute(object? parameter) => _execute();
    }
}