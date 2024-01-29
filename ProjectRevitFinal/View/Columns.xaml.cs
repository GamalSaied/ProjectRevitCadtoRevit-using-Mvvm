using ProjectRevitFinal.Revitcontext.Command;
using ProjectRevitFinal.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            // Set DataContext to the ViewModel
            this.DataContext = new ColumnsViewModel();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            // Handle button click event
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
                // Do something with the selected value
                MessageBox.Show($"Selected item: {selectedValue}");
            }
        }

    }
}
