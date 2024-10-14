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
    /// Interaction logic for Topic.xaml
    /// </summary>
    public partial class Topic : UserControl
    {
        public delegate void DoneEvent(object sender, string x);
        public event DoneEvent doneEvent;

        double marginBetween = 10;
        double inputDisplacement = 0;
        double currentTop = 0;

        Label[] firstDialogs;
        Image[] firstBubbles;

        int firstExplanation = 0;
        int firstExplanationCounts = 5;
        



        public Topic()
        {
            InitializeComponent();

            next_btn_MouseLeave(null, null);

            firstDialogs = new Label[firstExplanationCounts];
            firstBubbles = new Image[firstExplanationCounts];

            currentTop = Canvas.GetTop(parrotImg);
            inputDisplacement = parrotImg.Height;

            initFirstBubbles();
            firstExplanation++;
        }

        #region navigation

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if(firstExplanation< firstExplanationCounts)
            {
                doFirstExplanations();
                if (firstExplanation == 3)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                    myScroll.ScrollToBottom();
                }
                firstExplanation++;
            }
            else if(firstExplanation == firstExplanationCounts)
            {
                doInput();
                firstExplanation++;
            }
            else 
            {
                if(TopicText.Text.Length > 0) 
                {
                    Globals.MessageStructure.presentationTopic = TopicText.Text;
                    doneEvent(this, null);
                }
            }

        }

        void doFirstExplanations()
        {

            currentTop = currentTop + inputDisplacement + marginBetween;

            if (dialogCanvas.Height < currentTop)
            {
                dialogCanvas.Height = currentTop + parrotImg.Height;
            }
            Canvas.SetTop(parrotImg, currentTop);

            initFirstBubbles();

            myScroll.ScrollToBottom();
            inputDisplacement = parrotImg.Height;

           
        }

        void initFirstBubbles()
        {
            currentTop = Canvas.GetTop(parrotImg);

            firstBubbles[firstExplanation] = new Image();
            firstBubbles[firstExplanation].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            firstBubbles[firstExplanation].Height = 70;
            firstBubbles[firstExplanation].Width = 750;
            firstBubbles[firstExplanation].Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(firstBubbles[firstExplanation]);
            Canvas.SetTop(firstBubbles[firstExplanation], currentTop - 20);
            Canvas.SetLeft(firstBubbles[firstExplanation], 121);
            firstBubbles[firstExplanation].Visibility = Visibility.Visible;

            firstDialogs[firstExplanation] = new Label();
            firstDialogs[firstExplanation].Height = 60;
            firstDialogs[firstExplanation].Width = 650;
            dialogCanvas.Children.Add(firstDialogs[firstExplanation]);
            Canvas.SetTop(firstDialogs[firstExplanation], currentTop - 10);
            Canvas.SetLeft(firstDialogs[firstExplanation], 166);
            firstDialogs[firstExplanation].IsEnabled = false;

            switch (firstExplanation)
            {
                case 0:
                    firstDialogs[firstExplanation].Content = "Did you ever wonder what makes some presentations outstanding\n while others are so boring that everyone just wants to leave?";
                    break;
                case 1:
                    firstDialogs[firstExplanation].Content = "I did...";
                    break;
                case 2:
                    firstDialogs[firstExplanation].Content = "After reading tons of literature, interviewed experts and conducted\n research on the topic I learned some secrets.";
                    break;
                case 3:
                    firstDialogs[firstExplanation].Content = "Now I want to share these secrets with you and help you to prepare\n an outstanding presentation.";
                    break;
                case 4:
                    firstDialogs[firstExplanation].Content = "First let's provide a topic for the presentation.";
                    break;

            }

            if (firstExplanation > 0)
            {
                firstBubbles[firstExplanation - 1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
            }
        }

        void doInput()
        {
            TopicText.Visibility = Visibility.Visible;
            TopicLabel.Visibility = Visibility.Visible;
            TopicText.Text = Globals.MessageStructure.presentationTopic ;
        }


        #endregion




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
    }
}
