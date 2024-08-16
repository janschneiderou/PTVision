using PTVision.MessageCompositionViews;
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
    /// Interaction logic for MessageComposition.xaml
    /// </summary>
    public partial class MessageComposition : UserControl
    {

        bool before = false;
        bool intro = false;
        bool middle = false;
        bool conclusion = false;
        bool after = false;
        bool outro = false;
        bool topic = true;

        Before Before;
        After After;
        Introduction Introduction;
        Conclusion Conclusion;
        Middle Middle;
        OutroComposition Outro;
        Topic Topic;

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        public MessageComposition()
        {
            InitializeComponent();

            
            addViews();



            resetbuttons();
           

            setView();
        }

        void addViews ()
        {
            Before = new Before();
            Before.doneEvent += Before_doneEvent;
            viewGrids.Children.Add(Before);

            After  = new After();
            After.doneEvent += After_doneEvent;
            viewGrids.Children.Add(After);

            Introduction = new Introduction();
            Introduction.doneEvent += Introduction_doneEvent;
            viewGrids.Children.Add(Introduction);

            Conclusion = new Conclusion();
            Conclusion.doneEvent += Conclusion_doneEvent;
            viewGrids.Children.Add(Conclusion);


            Middle = new Middle();
            Middle.doneEvent += Middle_doneEvent;
            viewGrids.Children.Add(Middle);

            Outro = new OutroComposition();
            viewGrids.Children.Add(Outro);

            Topic = new Topic();
            Topic.doneEvent += Topic_doneEvent;
            viewGrids.Children.Add(Topic);
        }

       

        void resetbuttons ()
        {
            Before_btn_MouseLeave(null, null);
            Intro_btn_MouseLeave(null, null);
            Middle_btn_MouseLeave(null, null);
            conclussion_btn_MouseLeave(null, null);
            after_btn_MouseLeave(null, null);
        }

        #region doneEvents

        private void Topic_doneEvent(object sender, string x)
        {

            Before_btn_Click(null, null);
            BeforeImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_beforeO.png"));
        }


        private void Before_doneEvent(object sender, string x)
        {
            
            after_btn_Click(null, null);
            Before_btn_MouseLeave(null, null);
            

        }

        private void After_doneEvent(object sender, string x)
        {
            Intro_btn_Click(null, null);
            after_btn_MouseLeave(null, null);


        }

        private void Introduction_doneEvent(object sender, string x)
        {
            conclussion_btn_Click(null, null);
            Intro_btn_MouseLeave(null, null);
        }

        private void Conclusion_doneEvent(object sender, string x)
        {
            Middle_btn_Click(null, null);
            conclussion_btn_MouseLeave(null, null);
        }

        private void Middle_doneEvent(object sender, string x)
        {
            outro = true;
            middle = false;
            Middle_btn_MouseLeave(null, null);
            setView();
        }


        #endregion

        void setView()
        {
           

           if(before) 
           {
                Before.Visibility= Visibility.Visible;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility= Visibility.Collapsed;
                Conclusion.Visibility= Visibility.Collapsed;
                Middle.Visibility= Visibility.Collapsed;
                Outro.Visibility= Visibility.Collapsed;
                Topic.Visibility = Visibility.Collapsed;

            }
           else if(intro) 
           {
               Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Visible;
                Conclusion.Visibility = Visibility.Collapsed;
                Middle.Visibility = Visibility.Collapsed;
                Outro.Visibility = Visibility.Collapsed;
                Topic.Visibility = Visibility.Collapsed;
            }
           else if (middle) 
           {
               Before.Visibility = Visibility.Collapsed;
               After.Visibility = Visibility.Collapsed;
               Introduction.Visibility = Visibility.Collapsed;
               Conclusion.Visibility = Visibility.Collapsed;
                Middle.Visibility = Visibility.Visible;
                Outro.Visibility = Visibility.Collapsed;
                Topic.Visibility = Visibility.Collapsed;
            }
           else if (conclusion) 
           {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Collapsed;
                Conclusion.Visibility = Visibility.Visible;
                Middle.Visibility = Visibility.Collapsed;
                Outro.Visibility = Visibility.Collapsed;
                Topic.Visibility = Visibility.Collapsed;
            }
           else if (after)
           {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Visible;
                Introduction.Visibility = Visibility.Collapsed;
                Conclusion.Visibility = Visibility.Collapsed;
                Middle.Visibility = Visibility.Collapsed;
                Outro.Visibility = Visibility.Collapsed;
                Topic.Visibility = Visibility.Collapsed;
            }
           else if (outro)
            {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Collapsed;
                Conclusion.Visibility = Visibility.Collapsed;
                Middle.Visibility = Visibility.Collapsed;
                Outro.Visibility = Visibility.Visible;
                Topic.Visibility = Visibility.Collapsed;
                Outro.loadContent();
            }
           else if(topic)
            {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Collapsed;
                Conclusion.Visibility = Visibility.Collapsed;
                Middle.Visibility = Visibility.Collapsed;
                Outro.Visibility = Visibility.Collapsed;
                Topic.Visibility = Visibility.Visible;
            }

        }


        #region buttonAnimations
        private void Before_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            BeforeImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_beforeO.png"));
        }

        private void Before_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!before) 
            {
                BeforeImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_before.png"));
            }
            
        }

        private void Intro_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            IntroImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_introO.png"));
        }

        private void Intro_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!intro)
            {
                IntroImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_intro.png"));
            }
            
        }

        private void Middle_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            MiddleImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_middleO.png"));
        }

        private void Middle_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!middle)
            {
                MiddleImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_middle.png"));
            }
            
        }

        private void conclussion_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            ConclusionImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_conclusionO.png"));
        }

        private void conclussion_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!conclusion)
            {
                ConclusionImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_conclusion.png"));
            }
          
        }

        private void after_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            afterImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_afterO.png"));
        }

        private void after_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!after)
            {
                afterImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_after.png"));
            }
        }

        private void Button_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Button_CloseImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_save_closeO.png"));
        }

        private void Button_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_CloseImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_save_close.png"));
        }

        

        #endregion

        #region button clicks

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
            exitEvent(this, "");
        }




        private void Before_btn_Click(object sender, RoutedEventArgs e)
        {
            before = true;
            intro=false;
            middle = false;
            conclusion = false;
            after = false;
            topic = false;

            setView();
        }

        private void Intro_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = true;
            middle = false;
            conclusion = false;
            after = false;
            topic = false;

            setView();
            IntroImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_introO.png"));
        }

        private void Middle_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = true;
            conclusion = false; 
            after = false;
            topic = false;
            MiddleImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_middleO.png"));
            setView();

           
        }

        private void conclussion_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = false;
            conclusion = true;
            after = false;
            topic = false;

            setView();
            ConclusionImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_conclusionO.png"));
        }

        private void after_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = false;
            conclusion = false;
            after = true;
            topic = false;
            setView();

            afterImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_afterO.png"));

        }

        #endregion

        #region save stuff

        void saveFile ()
        {
            try
            {
                string path = "";
                path= Globals.usersPathComposition;
                Globals.scriptPath = path;
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                string myString = Newtonsoft.Json.JsonConvert.SerializeObject(Globals.MessageStructure);
                Console.WriteLine(myString);

                

               
                File.WriteAllText(path, myString);
            }
            catch (Exception ee)
            {
                int x = 0;
            }
        }

        #endregion




    }
}
