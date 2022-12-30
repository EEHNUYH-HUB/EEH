using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EEH.WPF
{
    public class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public Action<object> ExecuteHandler { get; set; }
        public Predicate<object> CanExecuteHandler { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteHandler != null)
            {
                return CanExecuteHandler(parameter);
            }

            return true;
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public void Execute(object parameter)
        {
            if(ExecuteHandler != null)
            {
                ExecuteHandler(parameter);
            }
        }
    }
}
