using AForge.Video;
using AForge.Video.DirectShow;
using Newtonsoft.Json;
using PTVision.LogObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
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
using Brushes = System.Windows.Media.Brushes;

namespace PTVision
{
    /// <summary>
    /// Interaction logic for VocalExercises.xaml
    /// </summary>
    public partial class VocalExercises : UserControl
    {
        int totalExercises = 8;
        int currentCount = 9;

        bool wordIdentified=false;

        bool TimeTostretch = true;

        string exerciseHelper = "";

        VocalExerciseSession vocalExerciseSession;

        enum currentCase 
            {
            RAISEARMS,
            LIPS_ONE,
            LIPS_TWO,
            TONGUE_ONE,
            TONGUE_TWO,
            WE_AH
            };

        currentCase currentExercise;

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        bool isPlaying = false;

        bool processReady = false;

        PoseAnalysisMedia analysisMedia;

        #region Camera
        VideoCaptureDevice videoSource;
        #endregion

        #region text2Speech
        SpeechSynthesizer speechSynthesizerObj;
        #endregion

        public VocalExercises()
        {
            InitializeComponent();
            analysisMedia = new PoseAnalysisMedia();
            currentExercise = currentCase.RAISEARMS;
            CounterLabel.Content = currentCount.ToString();
            InstructionLabel.FeedbackLabel.FontSize = 24;
            InstructionLabel.FeedbackLabel.Foreground = Brushes.Black;
            InstructionLabel.speechBubble.Width = 800;
            // InstructionLabel.FeedbackLabel.HorizontalAlignment = HorizontalAlignment.Left;
            InstructionLabel.FeedbackLabel.Width = 700;
            InstructionLabel.FeedbackLabel.Background = Brushes.Transparent;
             initWebcam();
        }






        #region pausePlay and Exit
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            doExitStuff();
        }

        public void doExitStuff()
        {
            DateTime currentTime = DateTime.Now;

            processReady = false;
            videoSource.NewFrame -= VideoSource_NewFrame1;
            videoSource.SignalToStop();

            videoSource.NewFrame -= VideoSource_NewFrame1;
            videoSource.SignalToStop();

            Globals.currentWord = 0;
            finishLoggingStuff();
            exitEvent(this, "");
        }

        private void Button_Pause_Play_Click(object sender, RoutedEventArgs e)
        {

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


        #region MainCycle

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
            int mod = currentCount % 3;
            switch(mod)
            {
                case 0:
                    exerciseHelper = "speaking from the nose"; 
                    break;
               case 1:
                    exerciseHelper = "speaking from the throat";
                    break;
                case 2:
                    exerciseHelper = "speaking from the chest";
                    break;

            }
            
            switch(currentExercise)
            {
                case currentCase.RAISEARMS:
                    doRaiseArmsAnalysis(body);
                    if(currentCount == 0)
                    {
                        currentCount = 9;
                        saveToFile();
                        currentExercise = currentCase.LIPS_ONE;
                        
                    }
                    break;
                case currentCase.LIPS_ONE:
                    doVocalAnalysis(1);
                    if (currentCount == 0)
                    {
                        currentCount = 9;
                        saveToFile();
                        currentExercise = currentCase.TONGUE_ONE;
                        
                    }
                    break;
                case currentCase.LIPS_TWO:
                    doVocalAnalysis(2);
                    if (currentCount == 0)
                    {
                        currentCount = 9;
                        saveToFile();
                        currentExercise = currentCase.TONGUE_ONE;
                        
                    }
                    break;
                case currentCase.TONGUE_ONE:
                    doVocalAnalysis(3);
                    if (currentCount == 0)
                    {
                        currentCount = 9;
                        saveToFile();
                        currentExercise = currentCase.TONGUE_TWO;
                        
                    }
                    break;
                case currentCase.TONGUE_TWO:
                    doVocalAnalysis(4);
                    if (currentCount == 0)
                    {
                        currentCount = 9;
                        saveToFile();
                        currentExercise = currentCase.WE_AH;
                        
                    }
                    break;
                case currentCase.WE_AH:
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                        InstructionLabel.FeedbackLabel.Content = "Good work!"; 
                        CounterLabel.Content = currentCount.ToString();

                    }));
                    break;
            }
            //if (f_isAnalysing == true)
            //{
            //    poseMedia.analysePose(body);
            //    volumeAnalysis.analyse();
            //    if (Globals.readyToPresent == true)
            //    {
            //        rulesAnalyserFIFO.AnalyseRules();
            //    }

            //    if (withScript == true && showScript == true)
            //    {
            //        doScriptStuff();
            //    }
            //}

        }


        void doVocalAnalysis(int exercise)
        {
            
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                InstructionLabel.FeedbackLabel.Content = "Say " + Globals.words[exercise].Text + " " + exerciseHelper; ;
                CounterLabel.Content = currentCount.ToString();

            }));
            if(wordIdentified==true)
            {
                wordIdentified = false;
                Task taskA = Task.Run(() => showFeedback());
                currentCount--;
            }
        }
        void doRaiseArmsAnalysis(string body)
        {
            analysisMedia.analyseRaiseArms(body);
            if (TimeTostretch==true)
            {
                
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                    InstructionLabel.FeedbackLabel.Content = "While inhaling, Raise arms and stretch yourself!";
                    CounterLabel.Content = currentCount.ToString();

                }));
               
                if(Globals.f_RaisedArms==true)
                {
                    TimeTostretch= false;
                    wordIdentified = false;
                }

            }
            else
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                    InstructionLabel.FeedbackLabel.Content = "Lower your arms and relax while saying \"AAAAAhhhhh\"";
                    

                }));
                if(Globals.f_RaisedArms==false && wordIdentified == true )
                {
                    TimeTostretch=true;
                    currentCount--;
                    wordIdentified = false;
                    Task taskA = Task.Run(() => showFeedback());
                    
                }
            }
        }

        void showFeedback()
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                FeedbackImg.Visibility= Visibility.Visible;

            }));
            Thread.Sleep(2000);
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                FeedbackImg.Visibility = Visibility.Collapsed;

            }));
        }


        #endregion

        #region Script Stuff
        public void recognizedWord(string text)
        {
           
            switch (currentExercise)
            {
                case currentCase.RAISEARMS:
                    if(text == Globals.words[0].Text)
                    {
                        wordIdentified = true;
                    }
                    break;
                case currentCase.LIPS_ONE:
                    if( text == Globals.words[1].Text)
                    {
                        wordIdentified = true; 
                    }
                    break;
                case currentCase.LIPS_TWO:
                    break;
                case currentCase.TONGUE_ONE:
                    if (text == Globals.words[3].Text)
                    {
                        wordIdentified = true;
                    }
                    break;
                case currentCase.TONGUE_TWO:
                    if (text == Globals.words[4].Text)
                    {
                        wordIdentified = true;
                    }
                    break;
                case currentCase.WE_AH:
                    break;
            }

        }
        #endregion

        #region logs

        void createSessionStuff()
        {
            vocalExerciseSession = new VocalExerciseSession();
            vocalExerciseSession.start = DateTime.Now;
            vocalExerciseSession.excercise = currentExercise.ToString();
            
        }

        void saveToFile()
        {
            createSessionStuff();
            string path = System.IO.Path.Combine(Globals.usersPathLogs + "\\VocalExercise.json");
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
                VocalExerciseSessions sessions = new VocalExerciseSessions();

                sessions.sessions.Add(vocalExerciseSession);
                string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                File.WriteAllText(path, myString);

            }
            else
            {
                string jsonOne = File.ReadAllText(path);
                VocalExerciseSessions sessions = JsonConvert.DeserializeObject<VocalExerciseSessions>(jsonOne);
                if (sessions == null)
                {
                    sessions = new VocalExerciseSessions();
                }
                sessions.sessions.Add(vocalExerciseSession);
                string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                File.WriteAllText(path, myString);
            }


        }


        void finishLoggingStuff()
        {

        }

        #endregion

        #region Checkboxes
        private void CB_Stretch_Relax_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_Stretch_Relax_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_Lips_Ba_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_Lips_Ba_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void CB_Tongue_La_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_Tongue_La_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_We_Ah_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CB_We_Ah_Checked(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region buttonAnimations



       

        private void Return_MouseEnter(object sender, MouseEventArgs e)
        {

            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_returnO.png"));
        }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_return.png"));
        }




        #endregion

        
    }
}
