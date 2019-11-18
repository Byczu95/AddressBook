using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MolkomRecruitmentTask.Commands
{
    public class RelayCommand : ICommand
    {
        #region Fields
        /// <summary> wykonanie polecenia </summary>
        readonly Action<object> execute;
        /// <summary> można wykonać polecenie </summary>
        readonly Predicate<object> canExecute;
        #endregion

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }
        #endregion

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion 
    }
}

