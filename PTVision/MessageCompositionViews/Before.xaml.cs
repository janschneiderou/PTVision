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
    /// Interaction logic for Before.xaml
    /// </summary>
    public partial class Before : UserControl
    {
        double marginBetween = 10;
        int dialogs = 2;
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


        public Before()
        {
            InitializeComponent();
           
            next_btn_MouseLeave(null, null);
           
            
            textInputs = new TextBox[inputs];
            SpeechBubbles = new Image[dialogs];
            textDialogs = new Label[dialogs];
            speechBubble.Visibility = Visibility.Visible;
            currentTop = Canvas.GetTop(parrotImg);
            inputDisplacement = parrotImg.Height;
            currentDialgog = 0;
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
            Canvas.SetTop(SpeechBubbles[currentDialgog], currentTop - 20 );
            Canvas.SetLeft(SpeechBubbles[currentDialgog], 121);
            SpeechBubbles[currentDialgog].Visibility= Visibility.Visible;

            textDialogs[currentDialgog] = new Label();
            textDialogs[currentDialgog].Height =  60;
            textDialogs[currentDialgog].Width = 650;
            dialogCanvas.Children.Add(textDialogs[currentDialgog]);
            Canvas.SetTop(textDialogs[currentDialgog], currentTop - 10 );
            Canvas.SetLeft(textDialogs[currentDialgog], 166);
            textDialogs[currentDialgog].IsEnabled = false;

            switch (currentDialgog)
            {
                case 0:
                    textDialogs[currentDialgog].Content = " For effective presentations it is really important to consider some aspects \n of the audience. So we will start by doing that now";
                    break;
                case 1:
                    textDialogs[currentDialgog].Content = "What does your audience already know about the topic?";
                    break;
                
            }
            
            if(currentDialgog > 0) 
            {
                SpeechBubbles[currentDialgog-1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if(currentDialgog ==0 )
            {
                double parrotTop = Canvas.GetTop(parrotImg);
               

                currentTop = currentTop + inputDisplacement + marginBetween;

                if (dialogCanvas.Height < currentTop)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                }
                Canvas.SetTop(parrotImg, currentTop);
                currentDialgog++;
                initSpeechBubbles();
                myScroll.ScrollToBottom();
                inputDisplacement = parrotImg.Height;

               
            }
           else  if(currentDialgog == 1) 
            {
                currentTop = currentTop + marginBetween;
                currentKnowledge = new CompositionInputs();
                currentKnowledge.initItems(Globals.CompositionInfo.AUDIENCE_PREVIOUS);
                dialogCanvas.Children.Add(currentKnowledge);
                Canvas.SetTop(currentKnowledge, currentTop);
                Canvas.SetLeft(currentKnowledge, 40);
                inputDisplacement =  currentKnowledge.Height;
                currentTop = currentTop + inputDisplacement;
                dialogCanvas.Height = currentTop - marginBetween;
                inputDisplacement = 0;
                myScroll.ScrollToBottom();
                currentDialgog++;
            }
            else if(currentDialgog == 2)
            {
                if(Globals.MessageStructure.audiencePrevious.Count > 0) 
                {
                    doneEvent(this, "");
                }
            }
            
        }

        #endregion
    }
}
