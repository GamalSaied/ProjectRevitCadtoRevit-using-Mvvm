using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model.AutoCAD;
using ProjectRevitFinal.Revitcontext.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRevitFinal.ViewModel
{
    public class LevelsViewModel : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;


        //public LevelsViewModel()
        //{
        //    // Add Buttons in Constractor
        //    importcommand = new mycommand(importcommand_Envoke);                                // Reference
        //    DrawColumn = new mycommand(DrawColumn_Envoke);
        //}

        //#region BUTTON_import
        ////---------------------------------------------- Mvvm ----------------------------------------------//
        ////1- Create property & binding from View --> Usercontrol --> Your Button --> Command{Binding $$}
        //public mycommand importcommand { get; }

        ////2- Envoke The Button When Click
        //public void importcommand_Envoke()
        //{
        //    ImportCad.GET_AutoCAD_FilePath();                      //++ Get AutoCAD FilePath
        //    ImportCad.Get_AutoCAD_Layers();                        //++ Get AutoCAD Layers

        //}

        ////3- Add Refrance on Constarctor at the Top 


        ////1- Create property & binding from View --> Usercontrol --> Your Button --> Command{Binding $$}
        //public mycommand DrawColumn { get; }

        ////2- Envoke The Button When Click
        //public void DrawLevels_Envoke()
        //{
        //    GetLevels.GetLevelsData();         //++ Get AutoCAD column type


        //}

        ////3- Add Refrance on Constarctor at the Top 
        ////--------------------------------------------- The End ---------------------------------------------//
        //#endregion
        //public event PropertyChangedEventHandler PropertyChanged;


    }
}
