using System;
using System.Windows.Input;

namespace CommandResolver
{
    public class Command<TypeArgument> : ICommand
    {
        private bool? canExecutePreviously;
        private readonly Action<TypeArgument> action;
        private readonly Func<TypeArgument, bool> canExecuteDelegate;

        public event EventHandler CanExecuteChanged;

        public Command(Action<TypeArgument> action, Func<TypeArgument, bool> canExecute)
        {
            this.action = action;
            this.canExecuteDelegate = canExecute;
        }

        public bool CanExecute(TypeArgument parameter)
        {
            var canExecute = this.canExecutePreviously ?? true;
            if (this.canExecuteDelegate != null)
            {
                canExecute = this.canExecuteDelegate(parameter);
                if (canExecute != this.canExecutePreviously)
                {
                    this.canExecutePreviously = canExecute;
                    this.RaiseCanExecuteChanged();
                }
            }
            
            return canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((TypeArgument)parameter);
        }

        public void Execute(TypeArgument parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.action(parameter);
            }
        }

        public void Execute(object parameter)
        {
            Execute((TypeArgument)parameter);
        }

        protected void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
