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

namespace PTVision.MessageCompositionViews
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {

        double marginBetween = 10;
        int dialogs = 7;
        int inputs = 2;
        Label[] textDialogs;
        Image[] SpeechBubbles;
        TextBox[] textInputs;
        int currentDialgog = 0;
        double currentTop = 0;
        CompositionInputs currentKnowledge;
        double inputDisplacement = 0;

        public delegate void DoneEvent(object sender, string x);
        public event DoneEvent doneEvent;


        public Introduction()
        {
            InitializeComponent();
            next_btn_MouseLeave(null, null);


            textInputs = new TextBox[inputs];
            SpeechBubbles = new Image[dialogs];
            textDialogs = new Label[dialogs];
            speechBubble.Visibility = Visibility.Visible;
            currentTop = Canvas.GetTop(parrotImg);
            inputDisplacement = parrotImg.Height;
            initSpeechBubbles();
        }

        void initSpeechBubbles()
        {




            currentTop = Canvas.GetTop(parrotImg);

            SpeechBubbles[currentDialgog] = new Image();
            SpeechBubbles[currentDialgog].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            SpeechBubbles[currentDialgog].Height = 70;
            SpeechBubbles[currentDialgog].Width = 750;
            SpeechBubbles[currentDialgog].Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(SpeechBubbles[currentDialgog]);
            Canvas.SetTop(SpeechBubbles[currentDialgog], currentTop - 20);
            Canvas.SetLeft(SpeechBubbles[currentDialgog], 121);
            SpeechBubbles[currentDialgog].Visibility = Visibility.Visible;

            textDialogs[currentDialgog] = new Label();
            textDialogs[currentDialgog].Height = 60;
            textDialogs[currentDialgog].Width = 650;
            dialogCanvas.Children.Add(textDialogs[currentDialgog]);
            Canvas.SetTop(textDialogs[currentDialgog], currentTop - 10);
            Canvas.SetLeft(textDialogs[currentDialgog], 166);
            textDialogs[currentDialgog].IsEnabled = false;

            switch (currentDialgog)
            {
                case 0:
                    textDialogs[currentDialgog].Content = "Great, now that we know more about your audience and what you want to reach with your \n presentation, we can start with prepering the actual presentation";
                    break;
                case 1:
                    textDialogs[currentDialgog].Content = "First, we'll make sure to interest your audience into your topic.";
                    break;
                case 2:
                    textDialogs[currentDialgog].Content = "A good chance for this is your introduction.";
                    break;
                case 3:
                    textDialogs[currentDialgog].Content = "Many people waste this chance by staring with \"Hello my name is Parrot and today we talk\n about xyz. This is my schedule. First we start with...\"";
                    break;
                case 4:
                    textDialogs[currentDialgog].Content = "See how everyone is already falling asleep or looking at their phones? ";
                    break;
                case 5:
                    textDialogs[currentDialgog].Content = "Let's make sure this won't happen to you!";
                    break;
                case 6:
                    textDialogs[currentDialgog].Content = "The good news is: there are multiple great options how to gain the audeience's interest. Just pick what\n feels like the best matvh for you and this presentation!";
                    break;
                case 7:
                    textDialogs[currentDialgog].Content = "Pick your Introduction type";
                    break;
            }

            if (currentDialgog > 0)
            {
                SpeechBubbles[currentDialgog - 1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
            }
        }


        #region buttonAnimations


        private void next_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            NextImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_NextO.png"));
        }

        private void next_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            NextImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Next.png"));
        }

        #endregion

        #region navigation 

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (currentDialgog < dialogs)
            {
                double parrotTop = Canvas.GetTop(parrotImg);


                currentTop = currentTop + inputDisplacement + marginBetween;

                if (dialogCanvas.Height < currentTop)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                }
                Canvas.SetTop(parrotImg, currentTop);
               
                initSpeechBubbles();
                myScroll.ScrollToBottom();
                inputDisplacement = parrotImg.Height;
                currentDialgog++;


            }
        }

            #endregion

    }
}
