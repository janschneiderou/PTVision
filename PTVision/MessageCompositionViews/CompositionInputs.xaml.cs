using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for CompositionInputs.xaml
    /// </summary>
    public partial class CompositionInputs : UserControl
    {
        Globals.CompositionInfo info;


        public CompositionInputs()
        {
            InitializeComponent();
            deleteButton_MouseLeave(null, null);
            addButton_MouseLeave(null, null);
            Height = 350;
        }

        public void initItems(Globals.CompositionInfo info)
        {
            this.info = info;
            switch(info)
            {
                case Globals.CompositionInfo.AUDIENCE_PREVIOUS:
                    foreach (string s in Globals.audiencePrevious)
                    {
                        listBoxContent.Items.Add(s);
                    }
                    break;
                case Globals.CompositionInfo.AUDIENCE_AFTER:
                    break;
            }

            
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            listBoxContent.Items.Add(inputText.Text);

            switch (info)
            {
                case Globals.CompositionInfo.AUDIENCE_PREVIOUS:
                    Globals.audiencePrevious.Add(inputText.Text);
                    break;
                case Globals.CompositionInfo.AUDIENCE_AFTER:
                    Globals.audienceAfter.Add(inputText.Text);
                    break;
            }

            
            inputText.Text = "";
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = listBoxContent.SelectedIndex;

            if(index != -1)
            {
                switch (info)
                {
                    case Globals.CompositionInfo.AUDIENCE_PREVIOUS:
                        Globals.audiencePrevious.RemoveAt(index);
                        break;
                    case Globals.CompositionInfo.AUDIENCE_AFTER:
                        Globals.audienceAfter.RemoveAt(index);
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
