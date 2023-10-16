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
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : UserControl
    {
        public List<string> users;
        public List<string> presentations;
        string tempPath;

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;
        public UserManagement()
        {
            InitializeComponent();
            userGrid.Visibility = Visibility.Visible;
            presentationGrid.Visibility = Visibility.Collapsed;


            users = new List<string>();
            presentations = new List<string>();

            GetDirectories();


            usersListBox.ItemsSource = users;
        }

        private void GetDirectories()
        {
            string executingDirectory = Directory.GetCurrentDirectory();
            // usersPath = executingDirectory + "\\users"; 
            Globals.usersPath = System.IO.Path.Combine(executingDirectory, "Users");

            bool exists = System.IO.Directory.Exists(Globals.usersPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(Globals.usersPath);
            try
            {
                List<string> temp;

                temp = Directory.GetDirectories(Globals.usersPath).ToList();
                foreach (string s in temp)
                {
                    int x = s.LastIndexOf("\\");
                    users.Add(s.Substring(x + 1));
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void getPresentationsDirectories()
        {

            tempPath = System.IO.Path.Combine(Globals.usersPath, "Presentations");

            bool exists = System.IO.Directory.Exists(tempPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(tempPath);
            try
            {
                List<string> temp;

                temp = Directory.GetDirectories(tempPath).ToList();
                foreach (string s in temp)
                {
                    int x = s.LastIndexOf("\\");
                    presentations.Add(s.Substring(x + 1));
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userNameTextBox.Text = (string)usersListBox.SelectedValue;
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.usersPath = System.IO.Path.Combine(Globals.usersPath, userNameTextBox.Text);




            bool exists = System.IO.Directory.Exists(Globals.usersPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Globals.usersPath);

            }

            presentationGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Collapsed;
            getPresentationsDirectories();
            presentationsListBox.ItemsSource = presentations;
        }

        private void presentationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            presentationNameTextBox.Text = (string)presentationsListBox.SelectedValue;
        }

        private void presentationButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.presentationPath = System.IO.Path.Combine(tempPath, presentationNameTextBox.Text);
            Globals.usersPathScripts = System.IO.Path.Combine(Globals.presentationPath, "Scripts");
            Globals.usersPathVideos = System.IO.Path.Combine(Globals.presentationPath, "Videos");
            Globals.usersPathLogs = System.IO.Path.Combine(Globals.presentationPath, "Logs");

            Globals.scriptPath = System.IO.Path.Combine(Globals.usersPathScripts + "\\Script.txt");

            bool exists = System.IO.Directory.Exists(Globals.presentationPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Globals.presentationPath);
                System.IO.Directory.CreateDirectory(Globals.usersPathScripts);
                System.IO.Directory.CreateDirectory(Globals.usersPathVideos);
                System.IO.Directory.CreateDirectory(Globals.usersPathLogs);
            }


            userGrid.Visibility = Visibility.Visible;
            presentationGrid.Visibility = Visibility.Collapsed;
            exitEvent(this, "");
        }

        #region button animations
        private void selectButton_MouseEnter(object sender, MouseEventArgs e)
        {
            selectButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_goO.png"));
        }


        private void selectButton_MouseLeave(object sender, MouseEventArgs e)
        {
            selectButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Go.png"));
        }

        private void presentationButton_MouseEnter(object sender, MouseEventArgs e)
        {
            presentationButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_goO.png"));
        }

        private void presentationButton_MouseLeave(object sender, MouseEventArgs e)
        {
            presentationButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Go.png"));
        }

        #endregion

    }
}
