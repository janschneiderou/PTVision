using PTVision.LogObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.RightsManagement;
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
using Label = System.Windows.Controls.Label;

namespace PTVision.MessageCompositionViews
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {

        double marginBetween = 10;
        int dialogs = 8;
        int inputs = 2;
        System.Windows.Controls.Label[] textDialogs;
        Image[] SpeechBubbles;
        TextBox[] textInputs;
        bool seen = true;
        bool flag1 = false;

        List <string> selections = new List <string>();
        //List<Image> selectionBubbles = new List<Image>();
        //List<Label> selectionDialogs = new List<Label>();

        bool noPointersSelected = true;
      

        int currentDialgog = 0;
        int currentSelection = 0;
        int totalSelections = 0;
        double currentTop = 0;
        CompositionSelections starters;
     
        double inputDisplacement = 0;

        public delegate void DoneEvent(object sender, string x);
        public event DoneEvent doneEvent;


        public Introduction()
        {
            InitializeComponent();
            next_btn_MouseLeave(null, null);


            textInputs = new TextBox[inputs];
            SpeechBubbles = new Image[dialogs];
            textDialogs = new System.Windows.Controls.Label[dialogs];
            speechBubble.Visibility = Visibility.Visible;
            currentTop = Canvas.GetTop(parrotImg);
            inputDisplacement = parrotImg.Height;
            if(Globals.MessageStructure.introductionStarters.Count > 0) 
            {
                seen = false;
            }
            initSpeechBubbles();
            currentDialgog++;
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

        void initSelectionBubbles()
        {
            Image myImage = new Image();
            myImage.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SpeechBubble.png"));
            myImage.Height = 70;
            myImage.Width = 750;
            myImage.Stretch = Stretch.Fill;

            dialogCanvas.Children.Add(myImage);

            
            Canvas.SetTop(myImage, currentTop - 20);
            Canvas.SetLeft(myImage, 121);
            myImage.Visibility = Visibility.Visible;

            Label myLabel = new Label();
            myLabel.Height = 60;
            myLabel.Width = 650;
            dialogCanvas.Children.Add(myLabel);
            Canvas.SetTop(myLabel, currentTop - 10);
            Canvas.SetLeft(myLabel, 166);
            myLabel.IsEnabled = false;
            switch(selections.ElementAt(currentSelection))
            {
                case "Tell as story":
                    myLabel.Content = "Write some pointers for your story";
                    break;
                case "Tell an impresive fact":
                    myLabel.Content = "Write some pointers for your fact";
                    break;
                case "Ask a question":
                    myLabel.Content = "Write your question";
                    break;

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
            if (currentDialgog < 7)
            {
                explanationDialogs();
                if (currentDialgog == 3) 
                {
                    dialogCanvas.Height = currentTop + parrotImg.Height;
                    myScroll.ScrollToBottom();
                }

            }
           else if(currentDialgog==7)
            {
                currentTop = currentTop + marginBetween;
                starters = new CompositionSelections();
                starters.initItems(Globals.CompositionInfo.INTRODUCTION);
                starters.addEvent += Starters_addEvent;
                starters.deleteEvent += Starters_deleteEvent;
                dialogCanvas.Children.Add(starters);
                totalSelections= Globals.MessageStructure.introductionStarters.Count;
                Canvas.SetTop(starters, currentTop);
                Canvas.SetLeft(starters, 40);
                inputDisplacement = starters.Height;
                currentTop = currentTop + inputDisplacement;
                dialogCanvas.Height = currentTop - marginBetween;
                inputDisplacement = 0;
                myScroll.ScrollToBottom();
                
                currentDialgog++;
            }
            else if (currentDialgog >= 8)
            {
                doInputsForIntroIdeas();
            }

               

        }

        void explanationDialogs()
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

        void lookForSelection()
        {
            
            foreach (IntroductionStarters introS in Globals.MessageStructure.introductionStarters)
            {

                string x = "";
                switch (introS.starter) 
                {
                    case Globals.StarterType.STORY:
                        x = "Tell as story";
                        break;
                    case Globals.StarterType.FACT:
                        x = "Tell an impresive fact";
                        break;
                    case Globals.StarterType.QUESTION:
                        x = "Ask a question";
                        break;
                }
                selections.Add(x);
               
            }
        }
        void doInputsForIntroIdeas()
        {
            if (Globals.MessageStructure.introductionStarters.Count > 0 )
            {
               
                if (currentSelection != totalSelections && noPointersSelected==true)
                {
                    double parrotTop = Canvas.GetTop(parrotImg);


                    currentTop = currentTop + inputDisplacement + marginBetween;

                    if (dialogCanvas.Height < currentTop)
                    {
                        dialogCanvas.Height = currentTop + parrotImg.Height;
                    }
                    Canvas.SetTop(parrotImg, currentTop);

                    lookForSelection();

                    initSelectionBubbles();
                    myScroll.ScrollToBottom();
                    inputDisplacement = parrotImg.Height;

                    addPointers();
                    noPointersSelected = false;
                }
                if(noPointersSelected==false && flag1==false)
                {
                    if(Globals.MessageStructure.introductionStarters.ElementAt(currentSelection).pointers.Count>0)
                    {
                        currentSelection++;
                        noPointersSelected=true;
                    }
                }
                if(totalSelections==currentSelection && seen == true)
                {
                    doneEvent(this, "");
                }
                else 
                {
                    seen = true;
                    flag1 = true;
                }
              
               

                
            }



        }

        void addPointers()
        {
            currentTop = currentTop + marginBetween;
            CompositionInputs pointers = new CompositionInputs();
            pointers.initItems(Globals.MessageStructure.introductionStarters.ElementAt(currentSelection));

            dialogCanvas.Children.Add(pointers);
            Canvas.SetTop(pointers, currentTop);
            Canvas.SetLeft(pointers, 40);
            inputDisplacement = pointers.Height;
            currentTop = currentTop + inputDisplacement;
            dialogCanvas.Height = currentTop - marginBetween;
            inputDisplacement = 0;
            myScroll.ScrollToBottom();
        }

        private void Starters_deleteEvent(object sender, string x)
        {
            if(totalSelections>0) 
            {
                totalSelections--;
                int i = 0;
                int j = -1;
                foreach (string l in selections)
                {
                    if(l==x)
                    {
                        j = i;
                        break;

                    }
                    i++;
                }
                if(j!=-1)
                {
                    selections.RemoveAt(j);
                }
            }
            
        }

        private void Starters_addEvent(object sender, string x)
        {
            selections.Add(x);
            totalSelections++;
        }

        #endregion

    }
}
