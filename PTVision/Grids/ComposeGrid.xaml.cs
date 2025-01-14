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
    /// Interaction logic for ComposeGrid.xaml
    /// </summary>
    /// 
   

    public partial class ComposeGrid : UserControl
    {
        public delegate void SelectionEvent(object sender, string x);
        public event SelectionEvent selectionEvent;

        public ComposeGrid()
        {
            InitializeComponent();
            ComposeButton_MouseLeave(null, null);
            promoImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Promo.png"));
        }

        #region selections
        private void SlidesButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "slides");
        }



        private void ScriptButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "script");
        }

        private void ComposeButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "compose");
        }

        #endregion

        #region MouseAnimations

        private void SlidesButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgSlidesButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_I_Slides_O.png"));
        }

        private void SlidesButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgSlidesButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_I_Slides.png"));
        }
        private void ScriptButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgScriptButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_W_W_Script_O.png"));
        }

        private void ScriptButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgScriptButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_W_W_Script.png"));
        }

        private void ComposeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgComposeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_compose1O.png"));
        }

        private void ComposeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgComposeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_compose1.png"));
        }



        #endregion

      
    }
}
