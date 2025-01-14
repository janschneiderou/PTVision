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
    /// Interaction logic for ReviewGrid.xaml
    /// </summary>
    public partial class ReviewGrid : UserControl
    {
        public delegate void SelectionEvent(object sender, string x);
        public event SelectionEvent selectionEvent;
        public ReviewGrid()
        {
            InitializeComponent();

            promoImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Promo.png"));
        }

        #region button animations
        private void ReviewButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgReviewButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Review_P_O.png"));
        }

        private void ReviewButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgReviewButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Review_P.png"));
        }

        #endregion

        #region selections
        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "review");
        }

        #endregion
    }
}
