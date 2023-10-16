﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.utilObjects
{
    internal class VideoCreation
    {
        string filename;
        string filenameAudio;
        string filenameCombined;
        Process process;

        public delegate void FileCreated(object sender, string x);
        public event FileCreated fileCreated;

        public VideoCreation()
        {
            foreach (var process in Process.GetProcessesByName("ffmpeg.exe"))
            {
                process.Kill();
            }
            deleteDuplicates();
            string[] files = Directory.GetFiles(Globals.usersPathVideos);
            List<string> ids = new List<string>();
            List<string> wavs = new List<string>();
            List<string> mp4s = new List<string>();



            //get Ids. 
            foreach (string file in files)
            {
                string id = "";
                if (file.IndexOf(".wav") != -1)
                {
                    id = file.Substring(0, file.IndexOf("."));

                    // filenameAudio = MainWindow.recordingPath + "\\" + MainWindow.recordingID + ".wav";
                    filenameAudio = System.IO.Path.Combine(Globals.usersPathVideos, id + ".wav");
                    //filenameCombined = MainWindow.recordingPath + "\\" + MainWindow.recordingID + "c.mp4";
                    filenameCombined = System.IO.Path.Combine(Globals.usersPathVideos, id + "c.mp4");
                    //filename = MainWindow.recordingPath + "\\" + MainWindow.recordingID + ".mp4";
                    filename = System.IO.Path.Combine(Globals.usersPathVideos, id + ".mp4");


                }
                if (filenameAudio != null)
                {
                    break;
                }
            }
            if (filenameAudio != null)
            {
                combineFiles();
            }


        }

        void deleteDuplicates()
        {
            List<string> ids = new List<string>();
            string[] files = Directory.GetFiles(Globals.usersPathVideos);
            foreach (string file in files)
            {
                if (file.IndexOf("c.mp4") != -1)
                {
                    //ids.Add(file.Substring(0, file.IndexOf("c.")));
                    string id = file.Substring(0, file.IndexOf("c."));
                    if (File.Exists(id + ".wav"))
                    {
                        try
                        {
                            File.Delete(id + ".wav");
                        }
                        catch
                        {

                        }

                    }
                    if (File.Exists(id + ".mp4"))
                    {
                        try
                        {
                            File.Delete(id + ".mp4");
                        }
                        catch
                        {

                        }
                    }
                    if (File.Exists(id + ".avi"))
                    {
                        try
                        {
                            File.Delete(id + ".avi");
                        }
                        catch
                        {

                        }
                    }

                }
            }


        }

        public void combineFiles()
        {
            // Thread thread = new Thread(combineFilesThread);
            // thread.Start();

            Task taskA = Task.Run(() => combineFilesThread());

        }

        void combineFilesThread()
        {
            try
            {


                string FFmpegFilename;

                FFmpegFilename = Directory.GetCurrentDirectory() + "\\HelpExes\\FFmpeg\\bin\\ffmpeg.exe";

                process = new Process();
                process.StartInfo.RedirectStandardOutput = true; /// try with false this might overflow... 
                process.StartInfo.RedirectStandardError = true; //try with false
                process.StartInfo.FileName = FFmpegFilename;


                process.StartInfo.Arguments = "-i " + filename + " -i " + filenameAudio + " -c:v copy -c:a aac -strict experimental " + filenameCombined + " -shortest";



                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;


                process.Start();

                string processOutput = null;
                while ((processOutput = process.StandardOutput.ReadLine()) != null)
                {
                    // do something with processOutput
                    Debug.WriteLine(processOutput);
                }

                process.Exited += Process_Exited;

                process.WaitForExit();

                int a = 1;
                a++;
            }
            catch
            {
                int x = 0;
                x++;
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            fileCreated(this, "");
        }

    }
}
