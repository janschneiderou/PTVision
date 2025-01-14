using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PTVision.Grids
{
    /// <summary>
    /// Interaction logic for ShineGrid.xaml
    /// </summary>
    public partial class ShineGrid : UserControl
    {
        public delegate void SelectionEvent(object sender, string x);
        public event SelectionEvent selectionEvent;

        public ShineGrid()
        {
            InitializeComponent();

            promoImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Promo.png"));
        }

        #region MouseAnimations

       

        private void TipsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgTipsButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Tips_O.png"));
        }

        private void TipsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgTipsButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Tips.png"));
        }

        private void NonVerbalButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgNonVerbalButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Nonverbal_S_O.png"));
        }

        private void NonVerbalButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgNonVerbalButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Nonverbal_S.png"));
        }


        #endregion


        #region selections
        private void NonVerbalButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "nonVerbal");
        }

        private void TipsButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "tips");
        }

       

        #endregion
    }
}
