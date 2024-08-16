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
    /// Interaction logic for CompositionSelections.xaml
    /// </summary>
    public partial class CompositionSelections : UserControl
    {
        Globals.CompositionInfo info;

        public delegate void AddEvent(object sender, string x);
        public event AddEvent addEvent;

        public delegate void DeleteEvent(object sender, string x);
        public event DeleteEvent deleteEvent;
        public CompositionSelections()
        {
            InitializeComponent();
            deleteButton_MouseLeave(null, null);
            addButton_MouseLeave(null, null);
            Height = 350;
        }

        public void initItems(Globals.CompositionInfo info)
        {
            this.info = info;
            switch (info)
            {
                case Globals.CompositionInfo.AUDIENCE_PREVIOUS:
                    foreach (string s in Globals.MessageStructure.audiencePrevious)
                    {
                        listBoxContent.Items.Add(s);
                    }
                    break;
                case Globals.CompositionInfo.AUDIENCE_AFTER:
                    foreach (string s in Globals.MessageStructure.audienceAfter)
                    {
                        listBoxContent.Items.Add(s);
                    }
                    break;
                case Globals.CompositionInfo.INTRODUCTION:
                    listBoxSelection.Items.Clear();
                    listBoxSelection.Items.Add("Tell as story");
                    listBoxSelection.Items.Add("Tell an impresive fact");
                    listBoxSelection.Items.Add("Ask a question");
                    foreach (IntroductionStarters i in Globals.MessageStructure.introductionStarters)
                    {
                        listBoxContent.Items.Add(getStringValue(i.starter));
                    }
                    break;
            }


        }

        string getStringValue (Globals.StarterType starterType)
        {
            string value="";

            switch(starterType)
            {
                case Globals.StarterType.STORY:
                    value = "Tell as story";
                    break;
                case Globals.StarterType.FACT:
                    value = "Tell an impresive fact";
                    break;
                case Globals.StarterType.QUESTION:
                    value = "Ask a question";
                    break;
            }

            return value;
        }
        Globals.StarterType getValue(string s)
        {
            Globals.StarterType value;
            value = new Globals.StarterType();

            switch (s)
            {
                case "Tell as story":
                    value = Globals.StarterType.STORY;
                    break;
                case "Tell an impresive fact":
                    value = Globals.StarterType.FACT;
                    break;
                case "Ask a question":
                    value = Globals.StarterType.QUESTION;
                    break;
            }    

            return value;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            listBoxContent.Items.Add((string)listBoxSelection.SelectedValue);

            switch (info)
            {
                case Globals.CompositionInfo.INTRODUCTION:
                    IntroductionStarters intro = new IntroductionStarters();
                    intro.starter = getValue((string)listBoxSelection.SelectedValue);
                    Globals.MessageStructure.introductionStarters.Add(intro);
                    addEvent(this, (string)listBoxSelection.SelectedValue);
                    break;
                
            }


            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = listBoxContent.SelectedIndex;

            if (index != -1)
            {
                switch (info)
                {
                    case Globals.CompositionInfo.INTRODUCTION:
                        Globals.MessageStructure.introductionStarters.RemoveAt(index);
                        deleteEvent(this, (string)listBoxContent.SelectedValue);
                        break;

                }
                listBoxContent.Items.RemoveAt(index);

            }

        }

        #region ButtonAnimations
        private void addButton_MouseEnter(object sender, MouseEventArgs e)
        {
            addImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_AddO.png"));
        }

        private void addButton_MouseLeave(object sender, MouseEventArgs e)
        {
            addImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Add.png"));
        }

        private void deleteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            deleteImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_DeleteO.png"));
        }

        private void deleteButton_MouseLeave(object sender, MouseEventArgs e)
        {
            deleteImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Delete.png"));
        }

        #endregion
    }
}
