using Newtonsoft.Json;
using PTVision.LogObjects;
using PTVision.utilObjects;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PTVision
{
    /// <summary>
    /// Interaction logic for PimpScript.xaml
    /// </summary>
    public partial class PimpScript : UserControl
    {
        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        bool isPlaying = false;
        bool videoLoaded = false;

        Techniques tech = new Techniques();

        List<string> improvements = new List<string>();
        List<string> techNames = new List<string>();

        FeedbacksSentences feedbackSentences;

        public PimpScript()
        {
            InitializeComponent();

            if (Globals.words !=null)
            {
                CurrentSentence.Text = Globals.words[Globals.currentWord].Text;
                if(Globals.words.Count > 1)
                {
                    NextSentenceLabel.Content = Globals.words[1].Text;
                }
            }
            getImprovemens();
            loadFeedbacks();
            QuestionsLabel.Content = "What do you want to communicate with your sentence? \n How would you make the delivery more powerful?";
        }

        void getImprovemens()
        {

            improvements = new List<string>();
            
            string tempPath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + "\\Data\\Techniques.json");
            string jsonOne = File.ReadAllText(tempPath);
            tech = new Techniques();
            tech = JsonConvert.DeserializeObject<Techniques>(jsonOne);
            if( tech != null)
            {
                for (int i = 0; i < tech.techniques.Count; i++)
                {
                    for(int j = 0; j< tech.techniques[i].Improvements.Count; j++)
                    {
                        bool found = false;
                        for (int k=0; k<improvements.Count; k++)
                        {
                           
                            if (tech.techniques[i].Improvements[j].Equals(improvements[k]))
                            {
                                found = true;
                                break;
                            }
                        }
                        if(found==false)
                        {
                            improvements.Add(tech.techniques[i].Improvements[j]);
                        }
                        
                    }
                    
                }
                foreach (string impro in improvements)
                {
                    ComboImprovement.Items.Add(impro);

                }
            }
            

        }

        private void Improvement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            descriptionTxt.Text = "";
            FeedbackTxt.Text = "";
            techNames = new List<string>();
            ComboTechnique.Items.Clear();
            foreach (Technique t in tech.techniques)
            {
                if (ComboImprovement.SelectedIndex != -1)
                {
                    foreach(string imp in t.Improvements)
                    if (imp.Equals(ComboImprovement.SelectedValue))
                    {
                        techNames.Add(t.Name);
                        ComboTechnique.Items.Add(t.Name);
                    }
                }
            }
        }

        private void Technique_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(Technique t in tech.techniques)
            {
                if (ComboTechnique.SelectedIndex != -1)
                {
                    if(t.Name.Equals(ComboTechnique.SelectedValue))
                    {
                        descriptionTxt.Text = t.Description;
                        FeedbackTxt.Text=t.Feedback;
                        loadVideo(t.Example);
                    }
                   
                }
            }
        }

        void loadVideo(string path)
        {
            try
            {

                string videoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + path); ;
                myVideo.Source = new System.Uri(videoPath);
                videoLoaded = true;
                myVideo.Stop();


            }

            catch
            {
                videoLoaded = false;
            }
        }


        void loadFeedbacks()
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



        #region clicks
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            


            foreach (FeedbackForSentences f4s in feedbackSentences.feedbacks)
            {
                if (f4s.sentence == CurrentSentence.Text)
                {
                    f4s.Explanation = descriptionTxt.Text;
                    f4s.feedbackKeywords = FeedbackTxt.Text;
                }
            }
            string path = System.IO.Path.Combine(Globals.usersPathLogs + "\\Feedbacks.json");
            string myString = Newtonsoft.Json.JsonConvert.SerializeObject(feedbackSentences);
            File.WriteAllText(path, myString);
            loadFeedbacks();

        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.words != null)
            {
                if (Globals.currentWord < Globals.words.Count-1)
                {
                    
                    Globals.currentWord++;
                    CurrentSentence.Text = Globals.words[Globals.currentWord].Text;

                    if (Globals.currentWord > 0)
                    {
                        PrevSentenceLabel.Content = Globals.words[Globals.currentWord - 1].Text;

                    }

                    if (Globals.words.Count > Globals.currentWord+1)
                    {
                        NextSentenceLabel.Content = Globals.words[Globals.currentWord + 1].Text;
                    }
                    else
                    {
                        NextSentenceLabel.Content = "The End.";
                    }
                }
               
            }

            
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.words != null)
            {
                if (Globals.currentWord > 0)
                {

                    Globals.currentWord--;
                    CurrentSentence.Text = Globals.words[Globals.currentWord].Text;
                    if (Globals.words.Count > Globals.currentWord)
                    {
                        NextSentenceLabel.Content = Globals.words[Globals.currentWord+1].Text;
                    }
                    if(Globals.currentWord > 0 ) 
                    {
                        PrevSentenceLabel.Content = Globals.words[Globals.currentWord - 1].Text;

                    }
                    else
                    {
                        PrevSentenceLabel.Content = "";
                    }

                }
               
               
            }
            
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            exitEvent(this, "");
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            myVideo.Stop();
            btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Play.png"));
            isPlaying = false;
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (videoLoaded)
            {
                if (isPlaying)
                {
                    myVideo.Pause();



                    btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Play.png"));
                    isPlaying = false;
                }
                else
                {

                    myVideo.Play();
                    btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_pause_play.png"));
                    isPlaying = true;
                }

            }
        }

        #endregion

        #region Button Animations
        private void Add_MouseEnter(object sender, MouseEventArgs e)
        {
            AddButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_addO.png"));
        }

        private void Add_MouseLeave(object sender, MouseEventArgs e)
        {
            AddButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_add.png"));
        }



        private void buttonPrevious_MouseEnter(object sender, MouseEventArgs e)
        {
            prevButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_BackO.png"));
        }

        private void buttonPrevious_MouseLeave(object sender, MouseEventArgs e)
        {
            prevButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Back.png"));
        }



        private void buttonNext_MouseEnter(object sender, MouseEventArgs e)
        {
            nextButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_NextO.png"));
        }

        private void buttonNext_MouseLeave(object sender, MouseEventArgs e)
        {
            nextButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Next.png"));
        }

        private void Return_MouseEnter(object sender, MouseEventArgs e)
        {

            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_returnO.png"));
        }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_return.png"));
        }

        private void Stop_MouseEnter(object sender, MouseEventArgs e)
        {
            btnStop.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_StopO.png"));
        }

        private void Stop_MouseLeave(object sender, MouseEventArgs e)
        {
            btnStop.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Stop.png"));
        }
        private void Play_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isPlaying)
            {
                btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_pause_playO.png"));
            }
            else
            {
                btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_PlayO.png"));
            }
        }

        private void Play_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isPlaying)
            {
                btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_pause_play.png"));
            }
            else
            {
                btnPlay.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Play.png"));
            }
        }









        #endregion

        #region hidden
        private void Button_Click(object sender, RoutedEventArgs e)
        {
               createTechniques();
        }

        void createTechniques()
        {
            Techniques techniques = new Techniques();

            Technique technique = new Technique();
            technique.Name = "Long Pause";
            technique.Example = "\\Videos\\one.mp4";
            technique.Description = "Long Pauses are extremely useful in many situations during presentations.";
            technique.Feedback = " 5 seconds Pause";
            technique.Modality = "Silence";
            technique.Improvements.Add("Suspence");
            technique.Improvements.Add("Understanding");
            technique.Improvements.Add("Regain Attention");

            techniques.techniques.Add(technique);

            technique = new Technique();
            technique.Name = "On the one hand";
            technique.Example = "\\Videos\\oneHand.mp4";
            technique.Description = "It is a good practice to support your verbal message with the same gesture such as: showing one hand while saying On the one hand";
            technique.Feedback = "One Hand Gesture";
            technique.Modality = "Gesture";
            technique.Improvements.Add("Structuring");

            techniques.techniques.Add(technique);

            technique = new Technique();
            technique.Name = "Ending Low tone";
            technique.Example = "\\Videos\\Low.mp4";
            technique.Description = "Ending with a low tone tends to command respect and can be used when calling the audience for some action";
            technique.Feedback = "Low Tone";
            technique.Modality = "Voice";
            technique.Improvements.Add("Respect");

            techniques.techniques.Add(technique);


            string myString = Newtonsoft.Json.JsonConvert.SerializeObject(techniques);

            string tempPath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + "\\Data");

            bool exists = System.IO.Directory.Exists(tempPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(tempPath);

            string path = tempPath + "\\Techniques.json";

            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
                
                File.WriteAllText(path, myString);

            }
            else
            {
                
                File.WriteAllText(path, myString);
            }


        }
        #endregion
    }
}
