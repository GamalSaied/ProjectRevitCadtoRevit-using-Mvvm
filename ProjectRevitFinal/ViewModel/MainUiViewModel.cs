using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            importcommand = new mycommand(ImportCadfile);
        }
        #endregion



        #region Properties

        private string _loadfilepath;

        public string Loadfilepath
        {
            get { return _loadfilepath; }
            set {

                _loadfilepath = value;

                OnPropertyChanged();


            }
        }
        //property to binding on the button importfile
        public mycommand importcommand { get; set; }

        //property for selectedlayer
        private int _SelectedLayer;

        public int SelectedLayer
        {
            get { return _SelectedLayer; }
            set { _SelectedLayer = value;
                OnPropertyChanged();
            }
        }

        #endregion




        #region Methods
        //method for onpropertychanged
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        }

        //method to import file from cad
        public void ImportCadfile()
        {
            ImportCad.importfilcadpath(Loadfilepath);
           

        }

        










        #endregion

    }
}
