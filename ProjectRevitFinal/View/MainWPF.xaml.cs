using Autodesk.Revit.UI;
using ProjectRevitFinal.Api;
using ProjectRevitFinal.Api.EventHandlers;
using ProjectRevitFinal.Model.AutoCAD;
using ProjectRevitFinal.ViewModel;
using ProjectRevitFinalApp.Windows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectRevitFinal.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWPF : Window
    {
        // Fields to store the ExternalEvent and handler instances.
        private readonly ExternalEvent _createLevelExternalEvent;
        private readonly ExternalEvent _deleteLevelExternalEvent;
        private readonly CreateLevelEventHandler _createLevelEventHandler;
        private readonly DeleteLevelEventHandler _deleteLevelEventHandler;
        LevelApiController _levelController;
        System.Collections.Generic.List<Domain.LevelModel> _levelDataList;

        private static MainWPF _GetData;
        public static MainWPF GetData { get => _GetData; internal set => _GetData = value; }

        public MainWPF()
        {
            _GetData = this;
            InitializeComponent();
            this.DataContext = new ColumnsViewModel();

        }
        public MainWPF(System.Collections.Generic.List<Domain.LevelModel> levelDataList, ExternalEvent createLevelExternalEvent, ExternalEvent deleteLevelExternalEvent,
            CreateLevelEventHandler createLevelEventHandler, DeleteLevelEventHandler deleteLevelEventHandler)
        {
            _GetData = this;
            InitializeComponent();
            this.DataContext = new ColumnsViewModel();
            // Store the instances in the fields.
            _createLevelExternalEvent = createLevelExternalEvent;
            _deleteLevelExternalEvent = deleteLevelExternalEvent;
            _createLevelEventHandler = createLevelEventHandler;
            _deleteLevelEventHandler = deleteLevelEventHandler;
            _levelDataList = levelDataList;


        }




        // MainWindow Styles 
        #region MainWindw Styles
        #region MainBorder 
        private void MainBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        #endregion
        #region Button --> 1 
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn1_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn1.BorderBrush = Brushes.Red;
        }

        private void Btn1_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn1.BorderBrush = Brushes.White;

        }
        #endregion
        #region Button --> 2 
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            LoadColumnsUserControl();

        }
        private void Btn2_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn2.BorderBrush = Brushes.Red;
        }

        private void Btn2_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn2.BorderBrush = Brushes.White;

        }
        #endregion
        #region Button --> 3 
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn3_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn3.BorderBrush = Brushes.Red;
        }

        private void Btn3_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn3.BorderBrush = Brushes.White;

        }
        #endregion
        #region Button --> 4 
        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            LoadGridsUserControl();

        }
        private void Btn4_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn4.BorderBrush = Brushes.Red;
        }

        private void Btn4_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn4.BorderBrush = Brushes.White;

        }
        #endregion
        #region Button --> 5 
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {

            LoadLevelsUserControl();

        }
        private void Btn5_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn5.BorderBrush = Brushes.Red;
        }

        private void Btn5_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn5.BorderBrush = Brushes.White;

        }
        #endregion
        #region Button --> 6 
        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Btn6_MouseEnter(object sender, MouseEventArgs e)
        {
            Border_Btn6.BorderBrush = Brushes.Red;
        }

        private void Btn6_MouseLeave(object sender, MouseEventArgs e)
        {
            Border_Btn6.BorderBrush = Brushes.White;

        }

        #endregion

        #endregion


        private void LoadLevelsUserControl()
        {
            // Create the Levels UserControl with the necessary parameters.
            Levels levelsControl = new Levels(_levelDataList, _createLevelEventHandler, _deleteLevelEventHandler,
                                              _createLevelExternalEvent, _deleteLevelExternalEvent);

            // Clear existing content and add the Levels UserControl to the StackPanel.
            Stack_Usercontrols.Children.Clear();
            Stack_Usercontrols.Children.Add(levelsControl);
        }

        private void LoadColumnsUserControl()
        {
            // Create the Columns UserControl with the necessary parameters.
            Columns ColumnsControl = new Columns();
            // Clear existing content and add the Levels UserControl to the StackPanel.
            Stack_Usercontrols.Children.Clear();
            Stack_Usercontrols.Children.Add(ColumnsControl);
            GetColumns.Get_AutoCAD_LayersColumns();
        }

        private void LoadHomeUserControl()
        {
            // Create the Columns UserControl with the necessary parameters.
            Columns ColumnsControl = new Columns();
            // Clear existing content and add the Levels UserControl to the StackPanel.
            Stack_Usercontrols.Children.Clear();
            Stack_Usercontrols.Children.Add(ColumnsControl);
        }
        private void LoadGridsUserControl()
        {
            // Create the Columns UserControl with the necessary parameters.
            Grids GridsControl = new Grids();
            // Clear existing content and add the Levels UserControl to the StackPanel.
            Stack_Usercontrols.Children.Clear();
            Stack_Usercontrols.Children.Add(GridsControl);
            GetGrids.Get_AutoCAD_LayersGrids();
        }

    }
}
