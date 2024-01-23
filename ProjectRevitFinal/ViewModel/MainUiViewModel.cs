using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.ViewModel
{
    public class MainUiViewModel : INotifyPropertyChanged
    {

        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        #region Constructor
       
        public MainUiViewModel()
        {

        }
        #endregion



        #region Properties



        #endregion




        #region Methods
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        }
        #endregion

    }
}
