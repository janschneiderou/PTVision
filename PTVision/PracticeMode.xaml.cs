using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
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
using Newtonsoft.Json;
using PTVision.LogObjects;
using PTVision.utilObjects;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace PTVision
{
    /// <summary>
    /// Interaction logic for PracticeMode.xaml
    /// </summary>
    public partial class PracticeMode : UserControl
    {
        public bool withScript = false;

        int myImageHeight;
        int myImageWidth;

        bool f_isAnalysing;


        #region things for script

        bool showScript = true;

        public RecordingClass recordingClass;
        public SlideHandler slideHandler;

        FeedbacksSentences feedbackSentences;

        bool readyForManualFeedback = false;
        bool manualVissible = true;
        bool autoVissible = true;
        bool processReady = true;

        VolumeAnalysis volumeAnalysis;
        PoseAnalysisMedia poseMedia;
        RulesAnalyserFIFO rulesAnalyserFIFO;
        
        #endregion

        #region Camera
        VideoCaptureDevice videoSource;
        #endregion

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        #region text2Speech
        SpeechSynthesizer speechSynthesizerObj;
        #endregion

        
        
        

        public PracticeMode()
        {
            InitializeComponent();
            setupSize();
           
            myCountDown.startAnimation();
            speechSynthesizerObj = new SpeechSynthesizer();
            
            Globals.readyToPresent = false;

            myCountDown.startAnimation();

            lookSelections();

            if (withScript == false)
            {
                CB_Show_Script.IsEnabled = false;
                CB_Show_Script.IsChecked = false;
                CB_Show_Script_Checked(null, null);

            }

            slideHandler = new SlideHandler();
            slideHandler.init();


            if (Globals.SlideConfigs.Count > 0)
            {
                SlideImage.Source = new BitmapImage(new Uri(Globals.SlideConfigs[SlideHandler.getCurrentSlide(Globals.currentWord)].fileName));
            }

            volumeAnalysis = new VolumeAnalysis();
            poseMedia = new PoseAnalysisMedia();
            rulesAnalyserFIFO = new RulesAnalyserFIFO();
            f_isAnalysing = true;

            initWebcam();

            rulesAnalyserFIFO.feedBackEvent += RulesAnalyserFIFO_feedBackEvent;
            rulesAnalyserFIFO.correctionEvent += RulesAnalyserFIFO_correctionEvent;

        }

        #region setupSize

        void setupSize()
        {
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * .75;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * .75;

            mainGrid.Height = this.Height;
            mainGrid.Width = this.Width;

            imgFrame.Height = this.Height;
            imgFrame.Width = this.Width;




            if (imgFrame.Width >= 854)
            {
                myImage.Width = 854;
            }
            else
            {
                myImage.Width = this.Width - 350;
            }

            if (imgFrame.Height >= 480)
            {
                myImage.Height = 480;
            }
            else
            {
                myImage.Height = this.Height - 300;
            }



            Globals.SourceWidth = 854;
            Globals.SourceHeight = 480;

            int leftDisplacement = (int)((this.Width - myImage.Width) / 2);

           // Globals.SourceLeft = (int)myImage.Margin.Left + (int)(this.Margin.Left + (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width) / 2);
            Globals.SourceLeft = leftDisplacement + (int)(this.Margin.Left + (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width) / 2);
            Globals.SourceTop = (int)myImage.Margin.Top + (int)(this.Margin.Top + (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height) / 2); ;


        }

        public void lookSelections()
        {
            CB_Manual_Checked(null, null);
            CB_Auto_Checked(null, null);
            CB_Show_Script_Checked(null, null);



        }

        #endregion

        #region event catchers from the Analysis
        private void RulesAnalyserFIFO_correctionEvent(object sender, PresentationAction x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                FeedbackLabel.Visibility = Visibility.Collapsed;
            }));
        }

        private void RulesAnalyserFIFO_feedBackEvent(object sender, PresentationAction x)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                if (autoVissible == true)
                {
                    FeedbackLabel.Visibility = Visibility.Visible;
                    FeedbackLabel.FeedbackLabel.Content = x.message;
                }

            }));
        }

        #endregion

        #region FeedbacksForScript

        void loadFeedbacks()
        {   
            if(Globals.usersPathLogs !="")
            {
                string path = System.IO.Path.Combine(Globals.usersPathLogs + "\\Feedbacks.json");
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                    feedbackSentences = new FeedbacksSentences();

                    foreach (IdentifiedSentence sentence in Globals.practiceSession.sentences)
                    {
                        FeedbackForSentences f4s = new FeedbackForSentences("", "", "", sentence.sentence);
                        feedbackSentences.feedbacks.Add(f4s);
                    }

                    string myString = Newtonsoft.Json.JsonConvert.SerializeObject(feedbackSentences);
                    File.WriteAllText(path, myString);

                }
                else
                {
                    string jsonOne = File.ReadAllText(path);
                    feedbackSentences = JsonConvert.DeserializeObject<FeedbacksSentences>(jsonOne);
                    if (feedbackSentences == null)
                    {
                        feedbackSentences = new FeedbacksSentences();
                        foreach (IdentifiedSentence sentence in Globals.practiceSession.sentences)
                        {
                            FeedbackForSentences f4s = new FeedbackForSentences("", "", "", sentence.sentence);
                            feedbackSentences.feedbacks.Add(f4s);
                        }
                    }


                }
            }
           
            readyForManualFeedback = true;
        }

        void displayManualFeedback()
        {
            if (readyForManualFeedback == true)
            {
                bool noFeedback = true;
                // ManualFeedback.Visibility = Visibility.Visible;
                foreach (FeedbackForSentences f4s in feedbackSentences.feedbacks)
                {
                    if (f4s.sentence == ScriptLabel.Content.ToString())
                    {

                        ManualFeedback.FeedbackLabel.Content = f4s.feedbackKeywords;

                        if (f4s.feedbackKeywords == "" || f4s.feedbackKeywords == " ")
                        {
                            ManualFeedback.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ManualFeedback.Visibility = Visibility.Visible;
                            noFeedback = false;
                        }
                    }

                }
                if(noFeedback)
                {
                    ManualFeedback.Visibility = Visibility.Collapsed;
                }

            }
        }


        #endregion


        #region initWebcamStuff
        private void initWebcam()
        {

            processReady = true;

            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice(videoDevices[Globals.selectedCamera].MonikerString);

            videoSource.NewFrame += VideoSource_NewFrame1; ;
            videoSource.Start();

          

        }

        #endregion

        #region Receiving Webcam and analysis


        private void VideoSource_NewFrame1(object sender, NewFrameEventArgs eventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                System.Drawing.Image myCurrentImage = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                ((System.Drawing.Bitmap)myCurrentImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                myImage.Source = image;

            });
            if (processReady)
            {
                Bitmap wcImage = (Bitmap)eventArgs.Frame.Clone();
                Task taskA = Task.Run(() => sendInfo(wcImage));
                processReady = false;
            }
        }


        void sendInfo(Bitmap wcImage)
        {
            string response = "";
            try
            {
                
                NetworkStream stream = Globals.MediaPclient.GetStream();

                byte[] bStream = ImageToByte(wcImage);

                stream.Write(bStream, 0, bStream.Length);
                byte[] dataReceive = new byte[1];
                int numBytesRead = stream.Read(dataReceive, 0, dataReceive.Length);


                using (MemoryStream ms = new MemoryStream())
                {
                    do
                    {
                        stream.Read(dataReceive, 0, dataReceive.Length);
                        ms.Write(dataReceive, 0, dataReceive.Length);
                    } while (stream.DataAvailable);
                    response = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);


                }
            }
            catch (Exception exx)
            {
                int xcc = 1;
            }
            // stream.Close();
            Task taskA = Task.Run(() => doAnalysis(response));
            processReady = true;
        }

         byte[] ImageToByte(System.Drawing.Image iImage)
        {
            MemoryStream mMemoryStream = new MemoryStream();
            iImage.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return mMemoryStream.ToArray();
        }

        void doAnalysis(string body)
        {
            if (f_isAnalysing == true)
            {
                poseMedia.analysePose(body); 
                volumeAnalysis.analyse();
                if (Globals.readyToPresent == true)
                {
                    rulesAnalyserFIFO.AnalyseRules();
                }

                if (withScript == true && showScript == true)
                {
                    doScriptStuff();
                }
            }

        }


        #endregion
      

        #region do script stuff

        private void buttonSpeak_Click(object sender, RoutedEventArgs e)
        {
            speechSynthesizerObj.Dispose();
            if ((string)ScriptLabel.Content != "")
            {

                speechSynthesizerObj = new SpeechSynthesizer();

                speechSynthesizerObj.SpeakAsync((string)ScriptLabel.Content);

            }
        }


        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.currentWord > 0)
            {
                Globals.currentWord--;
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.currentWord < Globals.words.Count)
            {
                Globals.currentWord++;

            }
        }

        public void recognizedWord(string text)
        {

            if (Globals.currentWord < Globals.words.Count)
            {
                if (Globals.words[Globals.currentWord].Text == text
                    || Globals.words[Globals.currentWord].Text == " " + text
                    || Globals.words[Globals.currentWord].Text == text + " "
                    || Globals.words[Globals.currentWord].Text == " " + text + " ")
                {
                    Globals.currentWord++;

                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {

                        if (Globals.SlideConfigs.Count > 0)
                        {
                            SlideImage.Source = new BitmapImage(new Uri(Globals.SlideConfigs[SlideHandler.getCurrentSlide(Globals.currentWord)].fileName));
                        }


                    }));

                }
            }
            logRecognisedWord(text);
        }
        void doScriptStuff()
        {


            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                if (Globals.words.Count > Globals.currentWord)
                {


                    ScriptLabel.Content = Globals.words[Globals.currentWord].Text;

                    displayManualFeedback();



                }
                else
                {
                    ScriptLabel.Content = "The End";
                }

            }));
        }

        public void setWithScript()
        {
            withScript = true;
            CB_Show_Script.IsEnabled = true;
            CB_Show_Script.IsChecked = true;
            CB_Show_Script_Checked(null, null);
        }

        #endregion


        #region logs
        public void logRecognisedWord(string text)
        {
            if (Globals.practiceSession != null)
            {
                foreach (IdentifiedSentence identifiedSentence in Globals.practiceSession.sentences)
                {
                    if (identifiedSentence.sentence == text
                        || identifiedSentence.sentence == " " + text
                        || identifiedSentence.sentence == text + " "
                        || identifiedSentence.sentence == " " + text + " ")
                    {
                        identifiedSentence.wasIdentified = true;
                        identifiedSentence.start = Globals.sentenceStarted;
                        identifiedSentence.end = DateTime.Now.AddSeconds(0) - Globals.practiceSession.start;

                    }
                }
            }

        }

        #endregion

        #region checkboxes

        private void CB_Manual_Unchecked(object sender, RoutedEventArgs e)
        {
            manualVissible = false;
            ManualFeedback.Visibility = Visibility.Collapsed;
        }

        private void CB_Manual_Checked(object sender, RoutedEventArgs e)
        {
            manualVissible = true;
            ManualFeedback.Visibility = Visibility.Visible;
        }

        private void CB_Auto_Unchecked(object sender, RoutedEventArgs e)
        {
            autoVissible = false;
            FeedbackLabel.Visibility = Visibility.Collapsed;
        }

        private void CB_Auto_Checked(object sender, RoutedEventArgs e)
        {
            autoVissible = true;
        }

   

        private void CB_Show_Script_Checked(object sender, RoutedEventArgs e)
        {
            if (CB_Show_Script.IsChecked == false)
            {
                showScript = false;
                scrptCanvas.Visibility = Visibility.Collapsed;
                buttonBack.Visibility = Visibility.Collapsed;
                buttonNext.Visibility = Visibility.Collapsed;
                ScriptLabel.Visibility = Visibility.Collapsed;
                buttonSpeak.Visibility = Visibility.Collapsed;
            }
            else
            {

                showScript = true;
                scrptCanvas.Visibility = Visibility.Visible;
                buttonBack.Visibility = Visibility.Visible;
                buttonNext.Visibility = Visibility.Visible;
                ScriptLabel.Visibility = Visibility.Visible;
                buttonSpeak.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Pause Start and Exit
        private void Button_Pause_Play_Click(object sender, RoutedEventArgs e)
        {
            if (f_isAnalysing == true)
            {

                f_isAnalysing = false;
                Grid_Pause.Visibility = Visibility.Visible;
                DateTime currentTime = DateTime.Now;
                pauseLoggingStuff();
                
            }
            else
            {

                f_isAnalysing = true;
                Grid_Pause.Visibility = Visibility.Collapsed;
                DateTime currentTime = DateTime.Now;
                
            }


        }

        private void Button_yes_Exit_Click(object sender, RoutedEventArgs e)
        {
            doExitStuff();
        }

        private void Button_keep_practicing_Click(object sender, RoutedEventArgs e)
        {
            Grid_Pause.Visibility = Visibility.Collapsed;
            DateTime currentTime = DateTime.Now;
           

            f_isAnalysing = true;
        }

        public void doExitStuff()
        {
            DateTime currentTime = DateTime.Now;

            processReady = false;
            videoSource.NewFrame -= VideoSource_NewFrame1;
            videoSource.SignalToStop();

            videoSource.NewFrame -= VideoSource_NewFrame1;
            videoSource.SignalToStop();
            
            poseMedia = null;
            volumeAnalysis = null;
            rulesAnalyserFIFO = null;
            Globals.currentWord = 0;
            finishLoggingStuff();
            exitEvent(this, "");
        }

        private void myCountDown_countdownFinished(object sender)
        {
            myCountDown.Visibility = Visibility.Collapsed;
            DateTime currentTime = DateTime.Now;
            
            Globals.readyToPresent = true;
            startLoggingStuff();


        }

        private void startLoggingStuff()
        {
            Globals.practiceSession = new PracticeSession();
            Globals.practiceSession.scriptVisible = withScript;

            // Start VIDEO stuff
            recordingClass = new RecordingClass(Globals.practiceSession.videoId);
            recordingClass.startRecording();
            //End video Stuff

            foreach (Word word in Globals.words)
            {
                IdentifiedSentence identifiedSentence = new IdentifiedSentence(word.Text);
                Globals.practiceSession.sentences.Add(identifiedSentence);
            }
            loadFeedbacks();
        }

        private void pauseLoggingStuff()
        {
            try
            {
                foreach (PracticeLogAction pa in Globals.practiceSession.actions)
                {
                    TimeSpan tp = new TimeSpan();
                    if (pa.end == tp)
                    {
                        pa.end = DateTime.Now - Globals.practiceSession.start;
                    }
                }
            }
            catch
            {

            }

        }

        void saveToJSON()
        {
            string myString = Newtonsoft.Json.JsonConvert.SerializeObject(Globals.practiceSession);
            Console.WriteLine(myString);
        }
        private void finishLoggingStuff()
        {
            // saveToJSON();
            if(Globals.usersPathLogs !="")
            {
                Globals.practiceSession.end = DateTime.Now;
                string path = System.IO.Path.Combine(Globals.usersPathLogs + "\\PracticeSession.json");
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                    PracticeSessions sessions = new PracticeSessions();

                    sessions.sessions.Add(Globals.practiceSession);
                    string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                    File.WriteAllText(path, myString);

                }
                else
                {
                    string jsonOne = File.ReadAllText(path);
                    PracticeSessions sessions = JsonConvert.DeserializeObject<PracticeSessions>(jsonOne);
                    if (sessions == null)
                    {
                        sessions = new PracticeSessions();
                    }
                    sessions.sessions.Add(Globals.practiceSession);
                    string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                    File.WriteAllText(path, myString);
                }
                recordingClass.stopRecording();
                recordingClass.combineFiles();
            }
            
        }


        #endregion





        #region button animations
        private void Button_Pause_Play_MouseEnter(object sender, MouseEventArgs e)
        {
            Button_Pause_PlayImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_pause_playO.png"));
        }

        private void Button_Pause_Play_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_Pause_PlayImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_pause_play.png"));
        }

        private void buttonBack_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonBackImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_BackO.png"));
        }

        private void buttonBack_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonBackImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Back.png"));
        }


        private void buttonNext_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonNextImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_NextO.png"));
        }

        private void buttonNext_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonNextImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Next.png"));
        }

        private void buttonSpeak_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonSpeakImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_speakO.png"));
        }

        private void buttonSpeak_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonSpeakImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_speak.png"));
        }

        private void Button_yes_Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Button_yes_ExitImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_yesO.png"));
        }

        private void Button_yes_Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_yes_ExitImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_yes.png"));
        }

        private void Button_keep_practicing_MouseEnter(object sender, MouseEventArgs e)
        {
            Button_keep_practicingImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_noO.png"));
        }

        private void Button_keep_practicing_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_keep_practicingImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_no.png"));
        }

        #endregion

    }
}
