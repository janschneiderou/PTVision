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

namespace PTVision
{
    /// <summary>
    /// Interaction logic for FeedbackObject.xaml
    /// </summary>
    public partial class FeedbackObject : UserControl
    {
        public FeedbackObject()
        {
            InitializeComponent();
            speechBubble.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
        }
    }
}
