using PTVision.MessageCompositionViews;
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

namespace PTVision
{
    /// <summary>
    /// Interaction logic for MessageComposition.xaml
    /// </summary>
    public partial class MessageComposition : UserControl
    {

        bool before = true;
        bool intro = false;
        bool middle = false;
        bool conclusion = false;
        bool after = false;

        Before Before;
        After After;
        Introduction Introduction;

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        public MessageComposition()
        {
            InitializeComponent();

            BeforeImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_beforeO.png"));
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
        }

        private void Introduction_doneEvent(object sender, string x)
        {
            throw new NotImplementedException();
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

        #endregion

        void setView()
        {
           if(before) 
           {
                Before.Visibility= Visibility.Visible;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility= Visibility.Collapsed;
            }
           else if(intro) 
           {
               Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Visible;
            }
           else if (middle) 
           {
               Before.Visibility = Visibility.Collapsed;
               After.Visibility = Visibility.Collapsed;
               Introduction.Visibility = Visibility.Collapsed;
            }
           else if (conclusion) 
           {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Collapsed;
                Introduction.Visibility = Visibility.Collapsed;
           }
           else if (after)
           {
                Before.Visibility = Visibility.Collapsed;
                After.Visibility = Visibility.Visible;
                Introduction.Visibility = Visibility.Collapsed;
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
                ConclusionImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Conclusion.png"));
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
            exitEvent(this, "");
        }


        private void Before_btn_Click(object sender, RoutedEventArgs e)
        {
            before = true;
            intro=false;
            middle = false;
            conclusion = false;
            after = false;

            setView();
        }

        private void Intro_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = true;
            middle = false;
            conclusion = false;
            after = false;

            setView();
        }

        private void Middle_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = true;
            conclusion = false; 
            after = false;

            setView();
        }

        private void conclussion_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = false;
            conclusion = true;
            after = false;

            setView();
        }

        private void after_btn_Click(object sender, RoutedEventArgs e)
        {
            before = false;
            intro = false;
            middle = false;
            conclusion = false;
            after = true;

            setView();

            afterImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_afterO.png"));

        }

        #endregion




    }
}
