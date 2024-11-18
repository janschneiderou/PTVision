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
using WpfAnimatedGif;

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

        public void setPostureIcon ()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarPosture2.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }
        public void setStillIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarDance.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
            
        }
        public void setGesturesIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarGestures.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);

           
        }

        public void setLowerVolumeIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarSoftSpeaking.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
            
        }
        public void setLouderVolumeIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarSpeakLouder.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }

        public void setStartSpeakingIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarStartSpeaking.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
           
        }

        public void setStopSpeakingIcon()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarPauses.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }

        public void  setLowerArms()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarLowerArms.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }


        public void setRaiseArms()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarRaiseArms.gif");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }


        public void setStopHmmIcon()
        {
            parrotImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\parrot.png"));
        }

        public void setDefault()
        {
            parrotImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\parrot.png"));
        }
    }
}
