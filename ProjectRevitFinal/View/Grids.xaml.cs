using ProjectRevitFinal.ViewModel;
using System.Windows.Controls;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Grids.xaml
    /// </summary>
    public partial class Grids : UserControl
    {
        private static Grids _GetData;

        public static Grids GetData { get => _GetData; internal set => _GetData = value; }
        public Grids()
        {
            _GetData = this;
            InitializeComponent();
            this.DataContext = new GridsViewModel();
        }
    }
}
