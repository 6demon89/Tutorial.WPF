using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tutorial.WPF.Command
{
    public class CloseApplicationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Execute Shutdown applicaiton, allows us to inject logic before the applicaiton will be closed by user
        /// </summary>
        public void Execute(object parameter)
        {
            var result = new View.UserConfermationDialogResult("The applicaiton will be closed after you click 'OK'").ShowDialog();
            if (result.Value)
                App.Current.Shutdown();
        }
    }
}
