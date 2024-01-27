using System;
using System.Windows.Input;

namespace ProjectRevitFinal.Commands
{
    // Mvvm Class // Must be Here 
    public class mycommand : ICommand
    {

        public event EventHandler CanExecuteChanged;


        private readonly Action _excute;


        private readonly Predicate<object> _canExcute;

        #region Constructor

        public mycommand(Action excute, Predicate<object> canExcute = null)
        {

            _excute = excute;

            _canExcute = canExcute;


        }
        #endregion

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _excute();
        }
    }
}

