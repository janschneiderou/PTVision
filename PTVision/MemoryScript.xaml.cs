﻿using Newtonsoft.Json;
using PTVision.LogObjects;
using PTVision.utilObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for MemoryScript.xaml
    /// </summary>
    public partial class MemoryScript : UserControl
    {
        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        string[] lines;

        bool alreadyShowed = false;
        bool playing = false;

        string tempText;
        int currentSecond;

        MemorySession memorySession;

        public SlideHandler slideHandler;

        int currentLevel = 1;

        public MemoryScript()
        {
            InitializeComponent();
            Globals.currentWord = 0;

            slideHandler = new SlideHandler();
            slideHandler.init();
            if (Globals.SlideConfigs.Count > 0)
            {
                SlideImage.Source = new BitmapImage(new Uri(Globals.SlideConfigs[SlideHandler.getCurrentSlide(Globals.currentWord)].fileName));
            }

            lines = File.ReadAllLines(Globals.scriptPath);
        }

        #region gameLogic

        public void startMemory()
        {
            while (playing)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                if (currentSecond > 0)
                {
                    currentSecond--;
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    {

                        correctImage.Visibility = Visibility.Collapsed;
                        incorrectImage.Visibility = Visibility.Collapsed;

                    }));

                }
                else
                {
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    {

                        Run run;
                        run = new Run(lines[Globals.currentWord]);
                        run.Foreground = System.Windows.Media.Brushes.Red;
                        resultTextBlock.Inlines.Add(run);
                        run = new Run(System.Environment.NewLine);
                        resultTextBlock.Inlines.Add(run);
                        Globals.currentWord++;
                        alreadyShowed = false;
                        incorrectImage.Visibility = Visibility.Visible;

                    }));
                    displaySlide();


                    currentSecond = 10;
                }
                if (Globals.currentWord < lines.Length)
                {
                    if (alreadyShowed == false)
                    {
                        switch (currentLevel)
                        {
                            case 1:
                                tempText = lines[Globals.currentWord];
                                break;
                            case 2:
                                tempText = hideWord(1);
                                break;
                            case 3:
                                tempText = hideWord(2);
                                break;
                            case 4:
                                tempText = hideWord(3);
                                break;

                        }
                        alreadyShowed = true;
                    }





                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    {
                        countdownLabel.Content = currentSecond.ToString();
                        gameTextBlock.Text = tempText;

                    }));


                }
                else
                {
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    {
                        Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_start.png"));

                    }));


                    playing = false;
                    saveToFile();
                }
            }
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {

                gameTextBlock.Text = "";
                countdownLabel.Content = "";

            }));

        }

        public string hideWord(int wordsDeleted)
        {
            string workingString = lines[Globals.currentWord];
            string constructedString = "";
            int wordCount = 0;
            //find number of words
            List<string> words = new List<string>();
            List<int> deletedWords = new List<int>();
            string tempWord = "";
            for (int i = 0; i < workingString.Length; i++)
            {
                if (workingString[i] == ' ')
                {
                    wordCount++;
                    words.Add(tempWord);
                    tempWord = "";
                }
                else
                {
                    tempWord = tempWord + workingString[i];
                }
            }
            wordCount++;
            words.Add(tempWord);

            // extract random word
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < wordsDeleted; i++)
            {

                deletedWords.Add(random.Next(wordCount));
            }

            //Create string without the randomWords
            int currentWord = 0;
            foreach (string word in words)
            {
                bool isCurrentWordDeleted = false;
                foreach (int deletedWord in deletedWords)
                {
                    if (currentWord == deletedWord)
                    {
                        isCurrentWordDeleted = true;
                    }
                }
                if (isCurrentWordDeleted == true)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        constructedString = constructedString + "_";
                    }

                }
                else
                {
                    constructedString = constructedString + word;
                }

                constructedString = constructedString + " ";
                currentWord++;


            }

            return constructedString;
        }

        public void recongnizedWord(string text)
        {
            if (Globals.currentWord < Globals.words.Count)
            {
                if (Globals.words[Globals.currentWord].Text == text
                    || Globals.words[Globals.currentWord].Text == " " + text
                    || Globals.words[Globals.currentWord].Text == text + " "
                    || Globals.words[Globals.currentWord].Text == " " + text + " ")

                {
                    alreadyShowed = false;
                    setSentenceValue(true);
                    Globals.currentWord++;
                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {

                        Run run;
                        run = new Run(text);
                        run.Foreground = System.Windows.Media.Brushes.Green;
                        resultTextBlock.Inlines.Add(run);
                        run = new Run(System.Environment.NewLine);
                        resultTextBlock.Inlines.Add(run);
                        currentSecond = 10;
                        correctImage.Visibility = Visibility.Visible;
                    }));

                    displaySlide();

                }
            }
            else
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {


                    playing = false;

                }));
            }
        }

        #endregion

        #region Logging

        void createSessionStuff()
        {
            memorySession = new MemorySession();
            memorySession.start = DateTime.Now;
            memorySession.level = currentLevel;

            foreach (Word word in Globals.words)
            {
                IdentifiedSentence identifiedSentence = new IdentifiedSentence(word.Text);
                memorySession.sentences.Add(identifiedSentence);
            }
        }

        void setSentenceValue(bool wasIdentified)
        {
            memorySession.sentences[Globals.currentWord].wasIdentified = wasIdentified;
        }

        void saveToJSON()
        {
            string myString = Newtonsoft.Json.JsonConvert.SerializeObject(memorySession);
            Console.WriteLine(myString);
        }

        void saveToFile()
        {
            saveToJSON();
            string path = System.IO.Path.Combine(Globals.usersPathLogs + "\\Memory.json");
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
                MemorySessions sessions = new MemorySessions();

                sessions.sessions.Add(memorySession);
                string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                File.WriteAllText(path, myString);

            }
            else
            {
                string jsonOne = File.ReadAllText(path);
                MemorySessions sessions = JsonConvert.DeserializeObject<MemorySessions>(jsonOne);
                if (sessions == null)
                {
                    sessions = new MemorySessions();
                }
                sessions.sessions.Add(memorySession);
                string myString = Newtonsoft.Json.JsonConvert.SerializeObject(sessions);
                File.WriteAllText(path, myString);
            }


        }

        #endregion


        #region SlideStuff
        void displaySlide()
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {

                if (Globals.SlideConfigs.Count > 0)
                {
                    SlideImage.Source = new BitmapImage(new Uri(Globals.SlideConfigs[SlideHandler.getCurrentSlide(Globals.currentWord)].fileName));
                }


            }));
        }

        #endregion

        #region Interactions

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (playing == false)
            {
                countdownLabel.Visibility = Visibility.Visible;
                currentSecond = 10;
                playing = true;
                alreadyShowed = false;
                resultTextBlock.Text = "";
                Globals.currentWord = 0;

                displaySlide();
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Stop.png"));
                Thread thread = new Thread(startMemory);

                createSessionStuff();

                thread.Start();
            }
            else
            {
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_start.png"));
                countdownLabel.Visibility = Visibility.Collapsed;
                gameTextBlock.Text = "";
                tempText = "";
                Globals.currentWord = 0;
                saveToFile();
                playing = false;

            }
        }






        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentWord = 0;
            playing = false;
            exitEvent(this, "");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (radioLevel1.IsChecked == true)
            {
                currentLevel = 1;
            }

            else if (radioLevel2.IsChecked == true)
            {
                currentLevel = 2;
            }

            else if (radioLevel3.IsChecked == true)
            {
                currentLevel = 3;
            }
            else if (radioLevel4.IsChecked == true)
            {
                currentLevel = 4;
            }


        }

        #endregion

        #region Animation for buttons
        private void Return_MouseEnter(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_returnO.png"));
        }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_return.png"));
        }

        private void Button_Start_MouseEnter(object sender, MouseEventArgs e)
        {
            if (playing == false)
            {
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_startO.png"));
            }
            else
            {
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_StopO.png"));
            }

        }

        private void Button_Start_MouseLeave(object sender, MouseEventArgs e)
        {
            if (playing == false)
            {
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_start.png"));
            }
            else
            {
                Button_StartImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\btn_Stop.png"));
            }
        }

        #endregion
    }
}
