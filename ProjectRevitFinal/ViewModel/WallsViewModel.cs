using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model.AutoCAD;
using System.ComponentModel;

namespace ProjectRevitFinal.ViewModel
{
    public class WallsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WallsViewModel()
        {
            // Initialize commands in constructor
            DrawWalls = new mycommand(DrawWalls_Envoke);
        }

        //Create Walls
        public mycommand DrawWalls { get; }
        public void DrawWalls_Envoke()
        {
            Getwalls.Get_AutoCAD_Walls();         //++ 
        }

        //3- Add Refrance on Constarctor at the Top 
        //--------------------------------------------- The End ---------------------------------------------//
    }
}
