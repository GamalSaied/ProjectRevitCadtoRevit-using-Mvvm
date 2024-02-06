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
            //this.DataContext = new ColumnsViewModel();
            // Set DataContext to the ViewModel
            // Assuming GIF is an Image control in your XAML
            //LoadGif();
        }
        // Make gif into dll 
        //private void LoadGif()
        //{
        //    // Assuming the namespace of your project is MyProject
        //    string resourceName = "ProjectRevitFinal.Resources.MyGIF.gif";

        //    // Load the GIF from the embedded resource
        //    Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);



        //    if (imageStream != null)
        //    {
        //        BitmapImage bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = imageStream;
        //        bitmapImage.EndInit();

        //        OImage.Source = bitmapImage;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Could not load the GIF.");
        //    }
        //}


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
