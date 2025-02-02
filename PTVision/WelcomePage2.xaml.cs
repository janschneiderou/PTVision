﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using AForge.Video.DirectShow;
using PTVision.Grids;
using PTVision.utilObjects;
using WpfAnimatedGif;

namespace PTVision
{
    /// <summary>
    /// Interaction logic for WelcomePage2.xaml
    /// </summary>
    public partial class WelcomePage2 : UserControl
    {
        FilterInfoCollection connectedDevices;
        public VolumeCalibration volumeCalibration;
        public UserManagement userManagement;
        public SlideSelection slideSelection;
        public WorkingWithScript workingWithScript;
        public MemoryScript memoryScript;
        public PracticeMode practiceMode;
        public ReviewPractice reviewPractice;
        public PresentationTips presentationTips;
        public PimpScript pimpScript;
        public VocalExercises vocalExercises;
        public MessageComposition messageComposition;

        public Grids.ReviewGrid reviewGrid;
        public Grids.ComposeGrid composeGrid;
       
        public Grids.ShineGrid shineGrid;
        public Grids.PracticeGrid practiceGrid;
       


        LearningDesign learningDesign;

        #region speech stuff
        private SpeechToText speechToText;

        #endregion

        public VolumeAnalysis volumeAnalysis;

        bool Composing = false;
        bool Practicing = false;
        bool Shining = false;
        bool Reflecting = false;

        public WelcomePage2()
        {
            InitializeComponent();
            volumeAnalysis = new VolumeAnalysis();

            // initSpeech();

            connectedDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in connectedDevices)
            {

                cameraSelector.Items.Add(fi.Name);
            }
            cameraSelector.SelectedIndex = 0;
            volumeCalibration = new VolumeCalibration();
            myGrid.Children.Add(volumeCalibration);
            volumeCalibration.Margin = new Thickness(0, 30, 0, 50);
            volumeCalibration.Visibility = Visibility.Collapsed;
            volumeCalibration.VerticalAlignment = VerticalAlignment.Bottom;
            volumeCalibration.HorizontalAlignment = HorizontalAlignment.Left;

            addUserMagangement();


            learningDesign = new LearningDesign();
            promoImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Promo.png"));

            string uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\EasterEgg.png";
            parrotImg.Source = new BitmapImage(new Uri(uri));

        }


        #region LearningDesign

        void locateTutor()
        {
            Tutor.Visibility = Visibility.Visible;
            Tutor.InstructionLabel.Content = learningDesign.Tasks[0].description;

            //switch (learningDesign.Tasks[0].taskType)
            //{
            //    case LearningDesign.TaskType.SLIDESELECTION:
            //        Tutor.HorizontalAlignment = SlideSelectionButton.HorizontalAlignment;
            //        Tutor.VerticalAlignment = SlideSelectionButton.VerticalAlignment;

            //        Tutor.Margin = new Thickness(SlideSelectionButton.Margin.Left + 20, SlideSelectionButton.Margin.Top,
            //            SlideSelectionButton.Margin.Right, SlideSelectionButton.Margin.Bottom - 20);


            //        break;
            //    case LearningDesign.TaskType.WRITESCRIPT:
            //        Tutor.HorizontalAlignment = AddScriptGrid.HorizontalAlignment;
            //        Tutor.VerticalAlignment = AddScriptGrid.VerticalAlignment;

            //        Tutor.Margin = new Thickness(AddScriptGrid.Margin.Left + 100, AddScriptGrid.Margin.Top - 50,
            //            AddScriptGrid.Margin.Right, AddScriptGrid.Margin.Bottom);
            //        break;
            //    case LearningDesign.TaskType.PRACTICEWITHSCRIPT:
            //        Tutor.HorizontalAlignment = PracticeGrid.HorizontalAlignment;
            //        Tutor.VerticalAlignment = PracticeGrid.VerticalAlignment;

            //        Tutor.Margin = new Thickness(PracticeGrid.Margin.Left + 100, PracticeGrid.Margin.Top - 50,
            //            PracticeGrid.Margin.Right, PracticeGrid.Margin.Bottom);
            //        checkBoxScript.IsChecked = true;
            //        break;
            //    case LearningDesign.TaskType.PRACTICEWITHOUTSCRIPT:
            //        Tutor.HorizontalAlignment = PracticeGrid.HorizontalAlignment;
            //        Tutor.VerticalAlignment = PracticeGrid.VerticalAlignment;

            //        Tutor.Margin = new Thickness(PracticeGrid.Margin.Left + 100, PracticeGrid.Margin.Top - 50,
            //            PracticeGrid.Margin.Right, PracticeGrid.Margin.Bottom);
            //        checkBoxScript.IsChecked = false;
            //        break;
            //    case LearningDesign.TaskType.REVIEWPRESENTATION:
            //        Tutor.HorizontalAlignment = ReviewPracticeGrid.HorizontalAlignment;
            //        Tutor.VerticalAlignment = ReviewPracticeGrid.VerticalAlignment;

            //        Tutor.Margin = new Thickness(ReviewPracticeGrid.Margin.Left + 100, ReviewPracticeGrid.Margin.Top - 50,
            //            ReviewPracticeGrid.Margin.Right, ReviewPracticeGrid.Margin.Bottom);
            //        break;
            //    case LearningDesign.TaskType.MEMORY:
            //        Tutor.HorizontalAlignment = MemoriseScriptGrid.HorizontalAlignment;
            //        Tutor.VerticalAlignment = MemoriseScriptGrid.VerticalAlignment;

            //        Tutor.Margin = new Thickness(MemoriseScriptGrid.Margin.Left + 100, MemoriseScriptGrid.Margin.Top - 50,
            //            MemoriseScriptGrid.Margin.Right, MemoriseScriptGrid.Margin.Bottom);
            //        break;

            //}
        }

        #endregion

        #region handling configurations

        private void languageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the selected item is a ComboBoxItem
            if (languageSelector.SelectedItem is ComboBoxItem comboBoxItem)
            {
                // Get the language tag from the Tag property of the ComboBoxItem
                string languageTag = comboBoxItem.Tag as string;

                // Update the selected language in the speech-to-text instance
                speechToText.UpdateSelectedLanguage(languageTag);
            }
        }

        private void cameraSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Globals.selectedCamera = cameraSelector.SelectedIndex;
        }



        private void Volume_Button_Click(object sender, RoutedEventArgs e)
        {

            if (volumeCalibration.Visibility != Visibility.Visible)
            {

                volumeCalibration.Visibility = Visibility.Visible;

            }
            else
            {
                volumeCalibration.Visibility = Visibility.Collapsed;
            }

        }

        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            addUserMagangement();
        }


        private void SlideSelectionButton_Click(object sender, RoutedEventArgs e)
        {

            slideSelection = new SlideSelection();
            myGrid.Children.Add(slideSelection);
            slideSelection.Margin = new Thickness(0, 0, 0, 0);
            slideSelection.VerticalAlignment = VerticalAlignment.Center;
            slideSelection.HorizontalAlignment = HorizontalAlignment.Center;
            slideSelection.Visibility = Visibility.Visible;
            slideSelection.exitEvent += SlideSelection_exitEvent;
          //  Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;

            if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.SLIDESELECTION)
            {
                learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            }
        }


        private void addUserMagangement()
        {
            userManagement = new UserManagement();
            myGrid.Children.Add(userManagement);
            userManagement.Margin = new Thickness(0, 0, 0, 0);
            userManagement.VerticalAlignment = VerticalAlignment.Center;
            userManagement.HorizontalAlignment = HorizontalAlignment.Center;
            userManagement.Visibility = Visibility.Visible;
            userManagement.exitEvent += UserManagement_exitEvent;
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (practiceMode != null)
            {
                practiceMode.doExitStuff();
            }
            if (vocalExercises != null)
            {
                vocalExercises.doExitStuff();
            }
            foreach (var process in Process.GetProcessesByName("ffmpeg"))
            {
                process.Kill();
            }
            System.Windows.Application.Current.Shutdown();
        }


        #endregion

        #region Exit Events

        private void WorkingWithScript_exitEvent(object sender, string x)
        {
            restartSpeech();
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                workingWithScript.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(workingWithScript);
              //  Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                locateTutor();
            }));
        }

        private void MessageComposition_exitEvent(object sender, string x)
        {
            
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                messageComposition.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(messageComposition);
                //  Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                locateTutor();
            }));
        }

        private void PracticeMode_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                practiceMode.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(practiceMode);
              //  Grid_for_Mode_Selection.Visibility = Visibility.Visible;

                locateTutor();
            }));

        }

        private void MemoryScript_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                memoryScript.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(memoryScript);
               // Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                locateTutor();
            }));
        }



        private void UserManagement_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                userManagement.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(userManagement);
             //   Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                
                UserManagementButton.IsEnabled = true;
                //locateTutor();
                initSpeech();
            }));
        }


        private void ReviewPractice_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                reviewPractice.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(reviewPractice);
              //  Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                locateTutor();
            }));
        }
        private void SlideSelection_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                slideSelection.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(slideSelection);
             //   Grid_for_Mode_Selection.Visibility = Visibility.Visible;
                locateTutor();
            }));
        }

        private void PresentationTips_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                presentationTips.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(presentationTips);
             //   Grid_for_Mode_Selection.Visibility = Visibility.Visible;
            }));
        }

        private void PimpScript_exitEvent(object sender, string x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                pimpScript.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(pimpScript);
              //  Grid_for_Mode_Selection.Visibility = Visibility.Visible;
            }));
        }

        private void VocalExercises_exitEvent(object sender, string x)
        {
            restartSpeech();
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                vocalExercises.Visibility = Visibility.Collapsed;
                myGrid.Children.Remove(vocalExercises);
             //   Grid_for_Mode_Selection.Visibility = Visibility.Visible;
            }));
        }

        #endregion


        #region Speech Analysis Init and delete
        public void initSpeech()
        {

            speechToText = new SpeechToText(languageSelector);
            speechToText.speechRecognizedEvent += SpeechRecognition_speechRecognizedEvent;
            speechToText.volumeReceivedEvent += SpeechRecognition_volumeReceivedEvent;
        }

        private void SpeechRecognition_volumeReceivedEvent(object sender, int audioLevel)
        {
            Globals.currentAudioLevel = audioLevel * Globals.audioFactor;
            if (volumeCalibration.Visibility == Visibility.Visible)
            {
                volumeCalibration.showWave();
            }
        }

        private void SpeechRecognition_speechRecognizedEvent(object sender, string text)
        {

            if (practiceMode != null)
            {
                if (practiceMode.Visibility == Visibility.Visible)
                {
                    practiceMode.recognizedWord(text);
                }

            }

            if (memoryScript != null)
            {
                if (memoryScript.Visibility == Visibility.Visible)
                {
                    memoryScript.recongnizedWord(text);
                }

            }
            if (vocalExercises != null)
            {
                if (vocalExercises.Visibility == Visibility.Visible)
                {
                    vocalExercises.recognizedWord(text);
                }
            }



        }

        public void restartSpeech()
        {


            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                initSpeech();
            }));
        }


        #endregion


        #region buttonAnimations

        private void Button_MouseEnter_Volume(object sender, MouseEventArgs e)
        {
            volumeButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_volume1O.png"));
        }

        private void Button_MouseLeave_Volume(object sender, MouseEventArgs e)
        {
            volumeButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_volume1.png"));
        }

        private void Button_MouseEnter_Exit(object sender, MouseEventArgs e)
        {
            buttonExitImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_exit1O.png"));
        }

        private void Button_MouseLeave_Exit(object sender, MouseEventArgs e)
        {
            buttonExitImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_exit1.png"));
        }

        private void UserManagementButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserManagementButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_userO.png"));
        }

        private void UserManagementButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UserManagementButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_user.png"));
        }

        private void ComposeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            
            imgComposeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Compose_O.png"));

        }

        private void ComposeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Composing == false)
                imgComposeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Compose.png"));
        }

        private void MakeItShineButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if(Shining==false)
                imgMakeItShineButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_MakeItShine.png"));
        }

        private void MakeItShineButton_MouseEnter(object sender, MouseEventArgs e)
        {
            
            imgMakeItShineButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_MakeItShine_O.png"));
        }
        private void PracticeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if(Practicing==false)
                imgPracticeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Practice.png"));
        }

        private void PracticeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgPracticeButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Practice_O.png"));
        }

        private void ReflectButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if(Reflecting==false)
                imgReflectButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Reflect.png"));
        }

        private void ReflectButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imgReflectButton.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Reflect_O.png"));
        }

        void resetNavigationButtons()
        {
            ComposeButton_MouseLeave(null, null);
            MakeItShineButton_MouseLeave(null, null);
            PracticeButton_MouseLeave(null, null);
            ReflectButton_MouseLeave(null, null);
        }

        #endregion


        #region Navigation Clicks
        private void ComposeButton_Click(object sender, RoutedEventArgs e)
        {
            if(Shining)
            {
                myGrid.Children.Remove(shineGrid);
                shineGrid.selectionEvent -= ShineGrid_selectionEvent;
            }
            if(Practicing)
            {
                myGrid.Children.Remove(practiceGrid);
                practiceGrid.selectionEvent -= PracticeGrid_selectionEvent;
            }
            if(Reflecting)
            {
                myGrid.Children.Remove(reviewGrid);
                reviewGrid.selectionEvent -= ReviewGrid_selectionEvent;
            }

            Composing = true;
            Practicing = false;
            Shining = false;
            Reflecting = false;

            resetNavigationButtons();

            composeGrid = new ComposeGrid();
            myGrid.Children.Add(composeGrid);
            composeGrid.Margin =  new Thickness(200, 0, 0, 60);
            composeGrid.selectionEvent += ComposeGrid_selectionEvent;
        }

        

        private void MakeItShineButton_Click(object sender, RoutedEventArgs e)
        {
            if(Composing)
            {
                myGrid.Children.Remove(composeGrid);
                composeGrid.selectionEvent -= ComposeGrid_selectionEvent;
            }
            if (Practicing)
            {
                myGrid.Children.Remove(practiceGrid);
                practiceGrid.selectionEvent -= PracticeGrid_selectionEvent;
            }
            if (Reflecting)
            {
                myGrid.Children.Remove(reviewGrid);
                reviewGrid.selectionEvent -= ReviewGrid_selectionEvent;
            }


            Composing = false;
            Practicing = false;
            Shining = true;
            Reflecting = false;

            resetNavigationButtons();

            shineGrid = new ShineGrid();
            myGrid.Children.Add(shineGrid);
            shineGrid.Margin = new Thickness(200, 0, 0, 60);
            shineGrid.selectionEvent += ShineGrid_selectionEvent;

            
            
        }

        

        private void PracticeButton_Click(object sender, RoutedEventArgs e)
        {

            if (Composing)
            {
                myGrid.Children.Remove(composeGrid);
                composeGrid.selectionEvent -= ComposeGrid_selectionEvent;
            }
            if (Shining)
            {
                myGrid.Children.Remove(shineGrid);
                shineGrid.selectionEvent -= ShineGrid_selectionEvent;
            }
            if (Reflecting)
            {
                myGrid.Children.Remove(reviewGrid);
                reviewGrid.selectionEvent -= ReviewGrid_selectionEvent;
            }

            Composing = false;
            Practicing = true;
            Shining = false;
            Reflecting = false;

            resetNavigationButtons();
            
            practiceGrid = new PracticeGrid();
            myGrid.Children.Add(practiceGrid);
            practiceGrid.Margin = new Thickness(200, 0, 0, 60);
            practiceGrid.selectionEvent += PracticeGrid_selectionEvent;
        }

        

        private void ReflectButton_Click(object sender, RoutedEventArgs e)
        {
            if (Shining)
            {
                myGrid.Children.Remove(shineGrid);
                shineGrid.selectionEvent -= ShineGrid_selectionEvent;
            }
            if (Practicing)
            {
                myGrid.Children.Remove(practiceGrid);
                practiceGrid.selectionEvent -= PracticeGrid_selectionEvent;
            }
            if (Composing)
            {
                myGrid.Children.Remove(composeGrid);
                composeGrid.selectionEvent -= ComposeGrid_selectionEvent;
            }

            Composing = false;
            Practicing = false;
            Shining = false;
            Reflecting = true;

            resetNavigationButtons();
            
            reviewGrid = new ReviewGrid();
            myGrid.Children.Add(reviewGrid);
            reviewGrid.Margin = new Thickness(200, 0, 0, 60);
            reviewGrid.selectionEvent += ReviewGrid_selectionEvent;
        }



        #endregion

        #region selectExercises

        private void ComposeGrid_selectionEvent(object sender, string x)
        {
            switch(x)
            {
                case "slides":
                    addSlides();

                    break;
                case "script":
                    addWorkWithScript();
                    break;
                case "compose":
                    addCompose();
                    break;
            }
        }

        private void ShineGrid_selectionEvent(object sender, string x)
        {
            switch (x)
            {
                case "nonVerbal":
                    addPimp();
                    break;
                case "tips":
                    addTips();
                    break;
            }
        }

        private void PracticeGrid_selectionEvent(object sender, string x)
        {
            switch (x)
            {
                case "practice":
                    addPracticeMode();

                    break;
                case "voice":
                    addVoice();
                    break;
                case "memory":
                    addMemory();
                    break;
            }
        }

        private void ReviewGrid_selectionEvent(object sender, string x)
        {
            switch (x)
            {
                case "review":
                    addReview();
                    break;

            }
        }

        #endregion

        #region Handling Exercises

        private void addPracticeMode()
        {
            
            practiceMode = new PracticeMode();
            //Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            //Tutor.Visibility = Visibility.Collapsed;
            //if (checkBoxScript.IsChecked == true)
            //{
                
            //}

            practiceMode.setWithScript();

            myGrid.Children.Add(practiceMode);


            practiceMode.Margin = new Thickness(0, 0, 0, 0);
            practiceMode.VerticalAlignment = VerticalAlignment.Center;
            practiceMode.HorizontalAlignment = HorizontalAlignment.Center;
            practiceMode.Visibility = Visibility.Visible;
            Globals.currentWord = 0;
            practiceMode.exitEvent += PracticeMode_exitEvent;

            //if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.PRACTICEWITHSCRIPT || learningDesign.Tasks[0].taskType == LearningDesign.TaskType.PRACTICEWITHOUTSCRIPT)
            //{
            //    learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            //}
        }

        private void addCompose()
        {
            // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;
            messageComposition = new MessageComposition();
            myGrid.Children.Add(messageComposition);

            messageComposition.Margin = new Thickness(0, 0, 0, 0);
            messageComposition.VerticalAlignment = VerticalAlignment.Center;
            messageComposition.HorizontalAlignment = HorizontalAlignment.Center;
            messageComposition.Visibility = Visibility.Visible;


            //if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.WRITESCRIPT)
            //{
            //    learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            //}

            messageComposition.exitEvent += MessageComposition_exitEvent;
        }

       

        private void addWorkWithScript()
        {
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;
            workingWithScript = new WorkingWithScript();
            myGrid.Children.Add(workingWithScript);

            workingWithScript.Margin = new Thickness(0, 0, 0, 0);
            workingWithScript.VerticalAlignment = VerticalAlignment.Center;
            workingWithScript.HorizontalAlignment = HorizontalAlignment.Center;
            workingWithScript.Visibility = Visibility.Visible;


            if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.WRITESCRIPT)
            {
                learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            }

            workingWithScript.exitEvent += WorkingWithScript_exitEvent;
        }

        private void addMemory()
        {
            //Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;
            memoryScript = new MemoryScript();
            myGrid.Children.Add(memoryScript);
            memoryScript.Margin = new Thickness(0, 0, 0, 0);
            memoryScript.VerticalAlignment = VerticalAlignment.Center;
            memoryScript.HorizontalAlignment = HorizontalAlignment.Center;
            Globals.currentWord = 0;

            if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.MEMORY)
            {
                learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            }

            memoryScript.exitEvent += MemoryScript_exitEvent;

        }

        private void addReview()
        {
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;
            reviewPractice = new ReviewPractice();
            myGrid.Children.Add(reviewPractice);
            reviewPractice.Margin = new Thickness(0, 0, 0, 0);
            reviewPractice.VerticalAlignment = VerticalAlignment.Center;
            reviewPractice.HorizontalAlignment = HorizontalAlignment.Center;
            reviewPractice.exitEvent += ReviewPractice_exitEvent;

            if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.REVIEWPRESENTATION)
            {
                learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            }

        }


        private void addTips()
        {
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            presentationTips = new PresentationTips();
            myGrid.Children.Add(presentationTips);
            presentationTips.Margin = new Thickness(0, 0, 0, 0);
            presentationTips.VerticalAlignment = VerticalAlignment.Center;
            presentationTips.HorizontalAlignment = HorizontalAlignment.Center;
            presentationTips.exitEvent += PresentationTips_exitEvent;
        }

        private void addPimp()
        {
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            pimpScript = new PimpScript();
            myGrid.Children.Add(pimpScript);
            pimpScript.Margin = new Thickness(0, 0, 0, 0);
            pimpScript.VerticalAlignment = VerticalAlignment.Center;
            pimpScript.HorizontalAlignment = HorizontalAlignment.Center;
            pimpScript.exitEvent += PimpScript_exitEvent;
        }

        private void addVoice()
        {
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            vocalExercises = new VocalExercises();
            Tutor.Visibility = Visibility.Collapsed;
            myGrid.Children.Add(vocalExercises);

            speechToText = new SpeechToText(languageSelector, true);
            speechToText.speechRecognizedEvent += SpeechRecognition_speechRecognizedEvent;
            speechToText.volumeReceivedEvent += SpeechRecognition_volumeReceivedEvent;


            vocalExercises.Margin = new Thickness(0, 0, 0, 0);
            vocalExercises.VerticalAlignment = VerticalAlignment.Center;
            vocalExercises.HorizontalAlignment = HorizontalAlignment.Center;
            vocalExercises.Visibility = Visibility.Visible;
            Globals.currentWord = 0;
            vocalExercises.exitEvent += VocalExercises_exitEvent; ;
        }

        private void addSlides()
        {

            slideSelection = new SlideSelection();
            myGrid.Children.Add(slideSelection);
            slideSelection.Margin = new Thickness(0, 0, 0, 0);
            slideSelection.VerticalAlignment = VerticalAlignment.Center;
            slideSelection.HorizontalAlignment = HorizontalAlignment.Center;
            slideSelection.Visibility = Visibility.Visible;
            slideSelection.exitEvent += SlideSelection_exitEvent;
           // Grid_for_Mode_Selection.Visibility = Visibility.Collapsed;
            Tutor.Visibility = Visibility.Collapsed;

            //if (learningDesign.Tasks[0].taskType == LearningDesign.TaskType.SLIDESELECTION)
            //{
            //    learningDesign.Tasks.Remove(learningDesign.Tasks[0]);
            //}
        }

        #endregion

        #region parrotAnimations
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

            Random rnd = new Random();
            int selected = rnd.Next(1, 10);

            string uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\EasterEgg.png";

            switch(selected)
            {
                case 0:
                    break;
                case 1:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarDance.gif";
                    break;
                case 2:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarGestures.gif";
                    break;
                case 3:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarLowerArms.gif";
                    break;
                case 4:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarMemory.gif";
                    break;
                case 5:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarPosture.gif";
                    break;
                case 6:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarPauses.gif";
                    break;
                case 7:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarStartSpeaking.gif";
                    break;
                case 8:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarRaiseArms.gif";
                    break;
                case 9:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\avatarSoftSpeaking.gif";
                    break;
                case 10:
                    uri = System.IO.Directory.GetCurrentDirectory() + "\\Images\\AvatarSpeakLouder.gif";
                    break;

            }


            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(uri);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\EasterEgg.png");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(parrotImg, image);
        }

        #endregion
    }
}
