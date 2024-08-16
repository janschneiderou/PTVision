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
    /// Interaction logic for Middle.xaml
    /// </summary>
    public partial class Middle : UserControl
    {
        state currentState = state.FIRSTEXPLANATION;
        enum state
        {
            FIRSTEXPLANATION,
            BEFORE_KNOWLEDGE,
            AFTER_KNOWLEDGE,
            MIDDLEMESSAGE
           
        }

        int firstExplanation = 0;
        int firstExplanationCounts = 4;

        Label[] firstDialogs;
        Image[] firstBubbles;

        double marginBetween = 10;
        double inputDisplacement = 0;
        double currentTop = 0;


        public delegate void DoneEvent(object sender, string x);
        public event DoneEvent doneEvent;

        public Middle()
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
            switch (currentState)
            {
                case state.FIRSTEXPLANATION:
                    doFirstExplanations();
                    break;
                case state.BEFORE_KNOWLEDGE:
                    doBeforeKnowledge();
                    break;
                case state.AFTER_KNOWLEDGE:
                    doAfterKnowledge();
                    break;
                case state.MIDDLEMESSAGE:
                    doMiddle();
                    break;

            }
        }


        #region firstExplanation

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

            currentState = state.BEFORE_KNOWLEDGE;
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
                    firstDialogs[firstExplanation].Content = "We have the inrtoduction to get the audience's interest and the end to summarize our main findings.\nNow it is time to create the middle part of the presentation.";
                    break;
                case 1:
                    firstDialogs[firstExplanation].Content = "Let's now look at what the audience knows before your presentation...";
                    break;
                case 2:
                    firstDialogs[firstExplanation].Content = "...and what they should know afterwards";
                    break;
                case 3:
                    firstDialogs[firstExplanation].Content = "How can you move them from what they know to what they do not know";
                    break;

            }

            if (firstExplanation > 0)
            {
                firstBubbles[firstExplanation - 1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
            }
        }

        #endregion

        #region BeforeKnowledge

        void doBeforeKnowledge()
        {
            showPreviousKnowledge();
            currentState = state.AFTER_KNOWLEDGE;
        }

        void showPreviousKnowledge()
        {
            string afterKnowledge = "Knowledge Before:\n";
            foreach (string s in Globals.MessageStructure.audiencePrevious)
            {
                afterKnowledge += s + "\n";
            }

            TextBox textBox = new TextBox();
            textBox.Text = afterKnowledge;
            dialogCanvas.Children.Add(textBox);
            currentTop = currentTop + parrotImg.Height + marginBetween;
            textBox.Background = Brushes.White;
            Canvas.SetTop(textBox, currentTop);
            Canvas.SetLeft(textBox, 121);
            //textBox.IsEnabled = false;
            textBox.Width = 750;
            textBox.Height = 160;
            currentTop = currentTop + textBox.Height + marginBetween;

            beforeTextFix.Text = afterKnowledge;

            dialogCanvas.Height = currentTop;
            myScroll.ScrollToBottom();
        }

        #endregion

        #region after Knowledge

        void doAfterKnowledge()
        {
            firstExplanation++;

            currentTop = currentTop + inputDisplacement + marginBetween * 3;

            if (dialogCanvas.Height < currentTop)
            {
                dialogCanvas.Height = currentTop + parrotImg.Height;
            }
            Canvas.SetTop(parrotImg, currentTop);

            initFirstBubbles();

            showAfterKnowledge();

            currentState = state.MIDDLEMESSAGE;
            
        }

        void showAfterKnowledge()
        {
            string afterKnowledge = "Knowledge gained:\n";
            foreach (string s in Globals.MessageStructure.audienceAfter)
            {
                afterKnowledge += s + "\n";
            }

            TextBox textBox = new TextBox();
            textBox.Text = afterKnowledge;
            dialogCanvas.Children.Add(textBox);
            currentTop = currentTop + parrotImg.Height + marginBetween;
            textBox.Background = Brushes.White;
            Canvas.SetTop(textBox, currentTop);
            Canvas.SetLeft(textBox, 121);
            //textBox.IsEnabled = false;
            textBox.Width = 750;
            textBox.Height = 160;
            currentTop = currentTop + textBox.Height + marginBetween;

            afterTextFix.Text = afterKnowledge;

            dialogCanvas.Height = currentTop;
            myScroll.ScrollToBottom();
        }

        #endregion

        #region middle

        void doMiddle()
        {
            arrow.Visibility = Visibility.Visible;
            arrow.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\arrow.png"));
            beforeTextFix.Visibility = Visibility.Visible;
            afterTextFix.Visibility= Visibility.Visible;

            if(firstExplanation == 2) 
            {
                firstExplanation++;

                currentTop = currentTop + inputDisplacement + marginBetween * 3;

                if (dialogCanvas.Height < currentTop)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                }
                Canvas.SetTop(parrotImg, currentTop);

                initFirstBubbles();

                middleInputs();
            }
            else
            {
                if(Globals.MessageStructure.middleStatements.Count>0)
                {
                    doneEvent(this, null);
                }
            }
        }

        void middleInputs()
        {
            CompositionInputs middleInputs = new CompositionInputs();
            middleInputs.initItems(Globals.CompositionInfo.MIDDLE);
            dialogCanvas.Children.Add(middleInputs);
            Canvas.SetTop(middleInputs, currentTop);
            Canvas.SetLeft(middleInputs, 40);
            inputDisplacement = middleInputs.Height;
            currentTop = currentTop + inputDisplacement;
            dialogCanvas.Height = currentTop - marginBetween;
            inputDisplacement = 0;
            myScroll.ScrollToBottom();
        }

        #endregion

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
