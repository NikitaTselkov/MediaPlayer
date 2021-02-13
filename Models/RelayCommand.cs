using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Models
{
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        readonly Func<object, object> _return;

        #endregion

        #region Constructors
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Func<object, object> Return)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _return = Return;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public object Result(object parameter)
        {
            return _return == null ? null : _return(parameter);
        }

        #endregion
    }
}
