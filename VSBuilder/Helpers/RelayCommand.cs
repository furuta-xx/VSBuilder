using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VSBuilder.Helpers
{
    public class RelayCommand : ICommand
    {
        protected readonly Action __execute;
        protected readonly Func<bool>? __canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            __execute = execute;
            __canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => __canExecute == null ? true : __canExecute();

        public void Execute(object? parameter) => __execute();

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
