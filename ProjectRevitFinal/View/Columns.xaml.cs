using System.Windows.Controls;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Columns.xaml
    /// </summary>
    public partial class Columns : UserControl
    {
        private static Columns _Getdate;

        public static Columns GetData { get => _Getdate; internal set => _Getdate = value; }

        public Columns()
        {
            _Getdate = this;
            InitializeComponent();
        }
    }
}
