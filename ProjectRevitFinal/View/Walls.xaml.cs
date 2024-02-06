using ProjectRevitFinal.Model.AutoCAD;
using ProjectRevitFinal.Revitcontext.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Walls.xaml
    /// </summary>
    public partial class Walls : UserControl
    {
        private static Walls _GetData;

        public static Walls GetData { get => _GetData; internal set => _GetData = value; }

        public Walls()
        {
            _GetData = this;
            InitializeComponent();
        }

        private void Layer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Similar logic to handle the selection changed for wall layers
            if (this.AutoCAD_Layer_Walls.SelectedItem != null)
            {
                // Assuming you have a method to clear and repopulate the wall types based on the selected layer
                // Clear ComboBox for wall types
                Walls.GetData.AutoCAD_Wall_Type.Items.Clear();
                string selectedLayer = this.AutoCAD_Layer_Walls.SelectedItem.ToString();

                // Assuming you have a similar way to get unique wall types for the selected layer
                List<string> uniqueWallTypes = ImportCad.AutoCAD_ListType
                    .Where(type => type.StartsWith(selectedLayer))
                    .Distinct()
                    .ToList();

                // Populate ComboBox with unique wall types
                foreach (var type in uniqueWallTypes)
                {
                    Walls.GetData.AutoCAD_Wall_Type.Items.Add(type); // Add Data to ComboBox
                }
            }
        }

        private void AutoCAD_Wall_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AutoCAD_Wall_Type.SelectedItem != null)
            {
                // Assuming you have a method in GetWalls (similar to GetColumns) to handle wall type selection
                // and possibly create Revit wall types based on the selection
                // GetWalls.CreateRevitWallTypes();
                // Reassign Revit wall data if necessary
                // GetWalls.Get_RevitWallTypes();

                // Placeholder for your logic to handle wall type selection changes
            }
        }

        // Implement additional logic as needed, similar to the Columns UserControl
    }
}
