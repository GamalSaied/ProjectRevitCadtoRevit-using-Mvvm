﻿using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model.AutoCAD;
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
            importcommand = new mycommand(importcommand_Envoke);                                // Reference
            DrawColumn = new mycommand(DrawColumn_Envoke);

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
            //GetColumns.testcol();
        }

        //3- Add Refrance on Constarctor at the Top 


        //1- Create property & binding from View --> Usercontrol --> Your Button --> Command{Binding $$}
        public mycommand DrawColumn { get; }

        //2- Envoke The Button When Click
        public void DrawColumn_Envoke()
        {
            GetColumns.GetColumnsData();         //++ Get AutoCAD column type

        }



        #endregion

    }
}