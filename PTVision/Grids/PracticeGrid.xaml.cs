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
    /// Interaction logic for PracticeGrid.xaml
    /// </summary>
    public partial class PracticeGrid : UserControl
    {
        public delegate void SelectionEvent(object sender, string x);
        public event SelectionEvent selectionEvent;

        public PracticeGrid()
        {
            InitializeComponent();
        }

        #region button animations
        private void MemoryButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgMemoryButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Memory_E_O.png"));
        }

      

        private void MemoryButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgMemoryButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Memory_E.png"));
        }

        private void VoiceButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgVoiceButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Voice_E_O.png"));
        }
        private void VoiceButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgVoiceButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Voice_E.png"));
        }

        private void PracticeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgPracticeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Practice_E_O.png"));
        }

        private void PracticeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imgPracticeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Practice_E.png"));
        }

        #endregion

        #region selections
        private void PracticeButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "practice");
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "voice");
        }

        private void MemoryButton_Click(object sender, RoutedEventArgs e)
        {
            selectionEvent(this, "memory");
        }

        #endregion
    }
}
