using ProjectRevitFinal.Commands;
using ProjectRevitFinal.Model.AutoCAD;
using ProjectRevitFinal.Revitcontext.Command;
using System.ComponentModel;

namespace ProjectRevitFinal.ViewModel
{
    public class WallsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WallsViewModel()
        {
            // Initialize commands in constructor
            ImportCommand = new mycommand(ImportCommandExecute);
            DrawWallsCommand = new mycommand(DrawWallsCommandExecute);
        }

        #region Command for Importing CAD Data
        public mycommand ImportCommand { get; private set; }

        private void ImportCommandExecute()
        {
            // Assuming you have methods similar to ImportCad class but for walls
            ImportCad.GET_AutoCAD_FilePath(); // To get AutoCAD FilePath and possibly layers for walls
            ImportCad.Get_AutoCAD_Layers();   // Adjust this method if needed for walls specifically
        }
        #endregion

        #region Command for Drawing Walls in Revit
        public mycommand DrawWallsCommand { get; private set; }

        private void DrawWallsCommandExecute()
        {
            // Assuming you have a method to process and draw walls from the selected AutoCAD data
            CreateWalls.CreateWallsFromCADLayers(); // This method needs to be implemented in CreateWalls class
        }
        #endregion

        // Implement the OnPropertyChanged method to raise the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
