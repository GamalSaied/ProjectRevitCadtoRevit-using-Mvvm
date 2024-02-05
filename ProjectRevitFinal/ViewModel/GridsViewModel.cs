using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model.AutoCAD;
using System.ComponentModel;

namespace ProjectRevitFinal.ViewModel
{
    public class GridsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GridsViewModel()
        {
            // Add Buttons in Constractor
            DrawGrids = new mycommand(DrawGrids_Envoke);
        }
        //Create grids
        public mycommand DrawGrids { get; }
        public void DrawGrids_Envoke()
        {
            GetGrids.DrawGrids();         //++ create fgrids
        }

        //3- Add Refrance on Constarctor at the Top 
        //--------------------------------------------- The End ---------------------------------------------//
    }
}
