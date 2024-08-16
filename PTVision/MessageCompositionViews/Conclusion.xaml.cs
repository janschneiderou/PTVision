using PTVision.LogObjects;
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
    /// Interaction logic for Conclusion.xaml
    /// </summary>
    public partial class Conclusion : UserControl
    {
        double marginBetween = 10;
        double inputDisplacement = 0;

        public delegate void DoneEvent(object sender, string x);
        public event DoneEvent doneEvent;
        int firstExplanation = 0;
        int firstExplanationCounts = 4;

        int bracketExplanation = 0;
        int bracketExplanationCounts = 1;

        Label[] textDialogs;
        Image[] SpeechBubbles;

        Label[] finalDialogs;
        Image[] finalBubbles;

        int currentFinal = 0;
        int totalFinal = 1;

        int openBracketsCounter = 0;

        double currentTop = 0;

        state currentState = state.FIRSTEXPLANATION;
        enum state
        {
            FIRSTEXPLANATION,
            OPENBRACKETS,
            RECAP,
            FINALMESSAGE
        }
        public Conclusion()
        {
            InitializeComponent();
            next_btn_MouseLeave(null, null);

            
            SpeechBubbles = new Image[firstExplanationCounts];
            textDialogs = new Label[firstExplanationCounts];

            finalBubbles = new Image[totalFinal];
            finalDialogs = new Label[totalFinal];

            speechBubble.Visibility = Visibility.Visible;
            currentTop = Canvas.GetTop(parrotImg);
            inputDisplacement = parrotImg.Height;


            initSpeechBubbles();
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
                case state.OPENBRACKETS:
                    doOpenBrackets();
                    break;
                case state.RECAP:
                    doRecap();
                    break;
                case state.FINALMESSAGE:
                    doFinalMessage();
                    break;

            }
        }


        void doFirstExplanations()
        {
            if(firstExplanation < firstExplanationCounts)
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

                if (firstExplanation == 3)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                    myScroll.ScrollToBottom();
                }

                firstExplanation++;
            }
            else
            {
                showIntro();
                currentState= state.OPENBRACKETS;
            }
            
        }

        #region firstExpl
        void showIntro()
        {
            string introText="";
            foreach(IntroductionStarters intros in Globals.MessageStructure.introductionStarters)
            {
                switch (intros.starter)
                {
                    case Globals.StarterType.STORY:
                        introText = introText + "Story:\n";
                             break;
                    case Globals.StarterType.FACT:
                        introText = introText + "Fact:\n";
                        break;
                    case Globals.StarterType.QUESTION:
                        introText = introText + "Question:\n";
                        break;
                }
                foreach (string s in intros.pointers)
                {
                    introText = introText + s + "\n";
                }
            }
            TextBox textBox = new TextBox();
            textBox.Text = introText;
            dialogCanvas.Children.Add(textBox);
            currentTop = currentTop + parrotImg.Height + marginBetween;
            Canvas.SetTop(textBox, currentTop );
            Canvas.SetLeft(textBox, 121);

            textBox.Width = 750;
            textBox.Height = 160;
            currentTop = currentTop + textBox.Height + marginBetween;

            dialogCanvas.Height = currentTop ;
            myScroll.ScrollToBottom();
            

        }

        void initSpeechBubbles()
        {

            currentTop = Canvas.GetTop(parrotImg);

            SpeechBubbles[firstExplanation] = new Image();
            SpeechBubbles[firstExplanation].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            SpeechBubbles[firstExplanation].Height = 70;
            SpeechBubbles[firstExplanation].Width = 750;
            SpeechBubbles[firstExplanation].Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(SpeechBubbles[firstExplanation]);
            Canvas.SetTop(SpeechBubbles[firstExplanation], currentTop - 20);
            Canvas.SetLeft(SpeechBubbles[firstExplanation], 121);
            SpeechBubbles[firstExplanation].Visibility = Visibility.Visible;

            textDialogs[firstExplanation] = new Label();
            textDialogs[firstExplanation].Height = 60;
            textDialogs[firstExplanation].Width = 650;
            dialogCanvas.Children.Add(textDialogs[firstExplanation]);
            Canvas.SetTop(textDialogs[firstExplanation], currentTop - 10);
            Canvas.SetLeft(textDialogs[firstExplanation], 166);
            textDialogs[firstExplanation].IsEnabled = false;

            switch (firstExplanation)
            {
                case 0:
                    textDialogs[firstExplanation].Content = "Great, the first and probably the most important part of your presentation is done.";
                    break;
                case 1:
                    textDialogs[firstExplanation].Content = "Now we'll create the end of your presentation.";
                    break;
                case 2:
                    textDialogs[firstExplanation].Content = "During the end, we should close any brackets that we opened during the introduction.";
                    break;
                case 3:
                    textDialogs[firstExplanation].Content = "Let's look at the introduction again and check if there are any open brackets.";
                    break;
                
            }

            if (firstExplanation > 0)
            {
                SpeechBubbles[firstExplanation - 1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
            }
        }

        #endregion

        #region openBrackets

        void doOpenBrackets()
        {
            if(openBracketsCounter == 0) 
            {
                currentTop = currentTop + marginBetween * 3;
                Canvas.SetTop(parrotImg, currentTop);
                initBracketExplanationBubble();
                bracketsInput();
                openBracketsCounter++;
            }
            
            if(Globals.MessageStructure.conclusionLogs.openBrackets.Count > 0) 
            {
                currentState = state.RECAP;
            }

        }

        void bracketsInput()
        {
            
            CompositionInputs openBrackets = new CompositionInputs();
            openBrackets.initItems(Globals.CompositionInfo.CONCLUSION_BRACKETS);
            dialogCanvas.Children.Add(openBrackets);
            Canvas.SetTop(openBrackets, currentTop);
            Canvas.SetLeft(openBrackets, 40);
            inputDisplacement = openBrackets.Height;
            currentTop = currentTop + inputDisplacement;
            dialogCanvas.Height = currentTop - marginBetween;
            inputDisplacement = 0;
            myScroll.ScrollToBottom();
            
        }
        void initBracketExplanationBubble()
        {
            Image bubble = new Image();

            bubble.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            bubble.Height = 70;
            bubble.Width = 750;
            bubble.Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(bubble);
            Canvas.SetTop(bubble, currentTop - 20);
            Canvas.SetLeft(bubble, 121);
            bubble.Visibility = Visibility.Visible;

            Label dialog = new Label();
            dialog.Height = 60;
            dialog.Width = 650;
            dialogCanvas.Children.Add(dialog);
            Canvas.SetTop(dialog, currentTop - 10);
            Canvas.SetLeft(dialog, 166);
            dialog.Content = "If there are any open brackets, you can note down below how to close them in the end.";


        }

        #endregion


        #region recap

        void doRecap()
        {
            


            currentTop = currentTop + marginBetween * 3;
            Canvas.SetTop(parrotImg, currentTop);
            initRecapBubble();
            showKnowledge();
            currentState = state.FINALMESSAGE;

            
        }

        void initRecapBubble() 
        {
            Image bubble = new Image();

            bubble.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            bubble.Height = 70;
            bubble.Width = 750;
            bubble.Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(bubble);
            Canvas.SetTop(bubble, currentTop - 20);
            Canvas.SetLeft(bubble, 121);
            bubble.Visibility = Visibility.Visible;

            Label dialog = new Label();
            dialog.Height = 60;
            dialog.Width = 650;
            dialogCanvas.Children.Add(dialog);
            Canvas.SetTop(dialog, currentTop - 10);
            Canvas.SetLeft(dialog, 166);
            dialog.Content = "Before we compose the rest of te end of your presentation, let's look where you wanted\n your audience to be after your presentation again.";
        }


        void showKnowledge()
        {
            string afterKnowledge = "";
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

            dialogCanvas.Height = currentTop;
            myScroll.ScrollToBottom();
        }

        #endregion

        #region Finalmessage
        void doFinalMessage()
        {
            if(currentFinal< totalFinal)
            {
                currentTop = currentTop + inputDisplacement + marginBetween*3;

                if (dialogCanvas.Height < currentTop)
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                }
                Canvas.SetTop(parrotImg, currentTop);

                initFinalBubbles();

                myScroll.ScrollToBottom();
                inputDisplacement = parrotImg.Height;
                currentFinal++;
            }
            else if (currentFinal == totalFinal) 
            {
                finalInputs();
                currentFinal++;
            }
            else if(Globals.MessageStructure.conclusionLogs.finalMessage.Count > 0) 
            {
                doneEvent(this, "");
            }
           
        }

        void finalInputs()
        {
            CompositionInputs finalInputs = new CompositionInputs();
            finalInputs.initItems(Globals.CompositionInfo.CONCLUSION_FINAL);
            dialogCanvas.Children.Add(finalInputs);
            Canvas.SetTop(finalInputs, currentTop);
            Canvas.SetLeft(finalInputs, 40);
            inputDisplacement = finalInputs.Height;
            currentTop = currentTop + inputDisplacement;
            dialogCanvas.Height = currentTop - marginBetween;
            inputDisplacement = 0;
            myScroll.ScrollToBottom();
        }

       

        void initFinalBubbles()
        {
            currentTop = Canvas.GetTop(parrotImg);

            finalBubbles[currentFinal] = new Image();
            finalBubbles[currentFinal].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            finalBubbles[currentFinal].Height = 70;
            finalBubbles[currentFinal].Width = 750;
            finalBubbles[currentFinal].Stretch = Stretch.Fill;
            dialogCanvas.Children.Add(finalBubbles[currentFinal]);
            Canvas.SetTop(finalBubbles[currentFinal], currentTop - 20);
            Canvas.SetLeft(finalBubbles[currentFinal], 121);
            finalBubbles[currentFinal].Visibility = Visibility.Visible;

            finalDialogs[currentFinal] = new Label();
            finalDialogs[currentFinal].Height = 60;
            finalDialogs[currentFinal].Width = 650;
            dialogCanvas.Children.Add(finalDialogs[currentFinal]);
            Canvas.SetTop(finalDialogs[currentFinal], currentTop - 10);
            Canvas.SetLeft(finalDialogs[currentFinal], 166);
            finalDialogs[currentFinal].IsEnabled = false;

            switch (currentFinal)
            {
                case 0:
                    finalDialogs[currentFinal].Content = "This is your main message. The conclusion is a good point to repeat your main message shortly \n and concisely again, so it sticks into the audence's head.";
                    break;
                case 1:
                    finalDialogs[currentFinal].Content = "How do you want to end your presentation?";
                    break;

            }

            if (currentFinal > 0)
            {
                finalBubbles[currentFinal - 1].Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubbleDone.png"));
            }
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
