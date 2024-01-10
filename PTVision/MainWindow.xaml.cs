using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace PTVision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WelcomePage welcomePage;
        WelcomePage2 welcomePage2;
        bool notConnectionYet = true;
        NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            initHelpServers();
            welcomePage = new WelcomePage();
            welcomePage2 = new WelcomePage2();
            //MainGrid.Children.Add(welcomePage);
            MainGrid.Children.Add(welcomePage2);
            Task taskA = Task.Run(() => initSocket());
            
            
        }


        void initSocket()
        {

            while (notConnectionYet)
            {
                try
                {
                    Globals.MediaPclient = new TcpClient();

                    // Define the IP address and port number of the server
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    int serverPort = 65432;

                    // Connect to the server using the IP endpoint
                    Globals.MediaPclient.Connect(new IPEndPoint(serverIP, serverPort));
                    Console.WriteLine("Connected to the server.");
                    stream = Globals.MediaPclient.GetStream();
                    notConnectionYet = false;
                    Thread.Sleep(100);

                }
                catch (Exception ex)
                {
                    int x = 1;
                    x++;
                }
            }

        }


        void initHelpServers()
        {
            initMediaP();
            
        }
        void initMediaP()
        {
            string mediaPPath = Environment.CurrentDirectory + "\\MediaP.txt";
            if (!File.Exists(mediaPPath))
            {
                using (StreamWriter sw = File.CreateText(mediaPPath))
                {
                    sw.WriteLine(Environment.CurrentDirectory+ "\\HelpExes\\mediaP.exe");
                }
            }
            string[] lines = File.ReadAllLines(mediaPPath);


            Globals.MediaPProcess = new Process();
            Globals.MediaPProcess.StartInfo.RedirectStandardOutput = false;
            Globals.MediaPProcess.StartInfo.RedirectStandardError = false;
            Globals.MediaPProcess.StartInfo.FileName = lines[0];



            Globals.MediaPProcess.StartInfo.UseShellExecute = false;
            Globals.MediaPProcess.StartInfo.CreateNoWindow = false;
            try
            {
                Globals.MediaPProcess.Start();

                Globals.MediaPProcess.Exited += MediaPProcess_Exited;
            }
            catch (Exception ex)
            {
                Globals.MediaPProcess.Kill();
            }
           
        }


        #region closing
        private void MediaPProcess_Exited(object? sender, EventArgs e)
        {
            if (Globals.ShuttingDown==false)
            {
                Globals.MediaPProcess.Exited -= MediaPProcess_Exited;
                initMediaP();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.MediaPProcess.Kill();
            if(welcomePage.practiceMode!=null)
            {
                welcomePage.practiceMode.doExitStuff();
              //  welcomePage2.practiceMode.doExitStuff();
            }
            if(welcomePage.vocalExercises!=null)
            {
                welcomePage.vocalExercises.doExitStuff();
              //  welcomePage2.vocalExercises.doExitStuff();
            }
            foreach (var process in Process.GetProcessesByName("mediaP"))
            {
                process.Kill();
            }

        }

        #endregion
    }
}
