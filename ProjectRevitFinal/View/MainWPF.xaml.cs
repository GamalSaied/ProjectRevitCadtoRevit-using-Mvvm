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
        public MainWPF()
        {
            InitializeComponent();
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
    }
}
