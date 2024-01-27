using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Revitcontext.Command;
using System.ComponentModel;

namespace ProjectRevitFinal.ViewModel
{
    public class ColumnsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ColumnsViewModel()
        {
            // Add Buttons in Constractor
            importcommand = new mycommand(importcommand_Envoke);                                // Referance
        }

        #region BUTTON_import
        //---------------------------------------------- Mvvm ----------------------------------------------//
        //1- Create property & binding from View --> Usercontrol --> Your Button --> Command{Binding $$}
        public mycommand importcommand { get; }

        //2- Envoke The Button When Click
        public void importcommand_Envoke()
        {
            ImportCad.GET_AutoCAD_FilePath();                      //++ Get AutoCAD FilePath
            ImportCad.Get_AutoCAD_Layers();                        //++ Get AutoCAD Layers

        }

        //3- Add Refrance on Constarctor at the Top 
        //--------------------------------------------- The End ---------------------------------------------//
        #endregion

    }
}