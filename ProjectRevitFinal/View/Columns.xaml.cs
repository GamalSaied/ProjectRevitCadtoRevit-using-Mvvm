using ProjectRevitFinal.Model.AutoCAD;
using ProjectRevitFinal.Revitcontext.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Columns.xaml
    /// </summary>
    public partial class Columns : UserControl
    {
        private static Columns _GetData;

        public static Columns GetData { get => _GetData; internal set => _GetData = value; }

        public Columns()
        {
            _GetData = this;
            InitializeComponent();
        }




        private void Layer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the ComboBox selection changed event here
            if (this.AutoCAD_Layer_Columns.SelectedItem != null)
            {
                // Clear Combobox 
                Columns.GetData.AutoCAD_Col_Type.Items.Clear();
                string selectedValue = this.AutoCAD_Layer_Columns.SelectedItem.ToString();
                // Get Unique AutoCAD Type 
                List<string> uniqueTypes = ImportCad.AutoCAD_ListType.Where(type => type.StartsWith(selectedValue))
                .Distinct()
                .ToList();
                // Insert uniqueLayers to Combobox 
                foreach (var type in uniqueTypes)
                {
                    Columns.GetData.AutoCAD_Col_Type.Items.Add(type.ToString()); // Add Data to Combobox
                }
                //------------------------------------------------------------------------------------

            }
        }

        private void AutoCAD_Col_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AutoCAD_Col_Type.SelectedItem != null)
            {
                //------------------------------------------------------------------------------------
                // Create Columns Type
                GetColumns.CreateRevitTypes();
                // Re assign Revit Data 
                GetColumns.Get_RevitTypes();

            }
        }
    }
}
