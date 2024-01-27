using ProjectRevitFinal.ViewModel;
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
    }
}
