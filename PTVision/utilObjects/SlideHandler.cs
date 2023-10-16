using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.utilObjects
{
    public class SlideHandler
    {
        
       
        public SlideHandler()
        {
            
        }

        public void init()
        {


            Globals.SlideConfigs = new List<SlideConfig>();
            getSlideNames();
            getScriptText();
            getStartIndexes();




        }

        public static int getCurrentSlide(int word)
        {
            int currentSlide = 0;
            for(int i = 0; i < Globals.SlideConfigs.Count; i++)
            {
                if (word >= Globals.SlideConfigs[i].startIndex)
                {
                    currentSlide = i;
                }
            }

            
            return currentSlide;
        }

        void getSlideNames()
        {
            string path = System.IO.Path.Combine(Globals.presentationPath + "\\Slides.txt");
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();

            }
            else
            {
                Globals.SlidesPath = File.ReadAllText(path);
            }
            if(Globals.SlidesPath != "" || Globals.SlidesPath != null)
            {
                bool exists = System.IO.Directory.Exists(Globals.SlidesPath);

                if ( exists)
                {
                    var files = from file in Directory.EnumerateFiles(Globals.SlidesPath) select file;

                    foreach (var file in files)
                    {
                        if (file.IndexOf("Slide") != -1 && file.IndexOf(".PNG") != -1)
                        {
                            SlideConfig slideConfig = new SlideConfig();
                            slideConfig.fileName = file;
                            Globals.SlideConfigs.Add(slideConfig);
                        }
                    }
                }
               
            }
            
        }

        void getScriptText()
        {
            int counter = 1;
            foreach(SlideConfig slideConfig in Globals.SlideConfigs)
            {
                string path = System.IO.Path.Combine(Globals.usersPathScripts + "\\Slide"+ counter+ ".txt");
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                    slideConfig.scriptText = "";

                }
                else
                {
                    slideConfig.scriptText = File.ReadAllText(path);
                }
                counter++;
            }
        }
        void getStartIndexes()
        {
            int counter = 0;
            if (Globals.SlideConfigs.Count > 0)
            {
                Globals.SlideConfigs[0].startIndex = 0;
                for (int i = 1; i < Globals.SlideConfigs.Count; i++)
                {
                    counter = counter + CountLines(Globals.SlideConfigs[i - 1].scriptText);
                    Globals.SlideConfigs[i].startIndex = counter;
                }
            }
            
            //foreach (SlideConfig slideConfig in SlideConfigs)
            //{
            //    counter = counter + CountLines(slideConfig.scriptText);
            //    slideConfig.startIndex = counter;
            //}
        }
        public void setStartIndexAndText(int currentSlide)
        {

        }

        public void updateScriptText(int slide)
        {
            int slideTemp = slide + 1;
            string path = System.IO.Path.Combine(Globals.usersPathScripts + "\\Slide" + slideTemp + ".txt");
            if(Globals.SlideConfigs.Count > 0)
            {
                File.WriteAllText(path, Globals.SlideConfigs[slide].scriptText);
            }
            
            getStartIndexes();
        }

        public void generateScript()
        {
            try
            {
                string path = "";
                path = System.IO.Path.Combine(Globals.usersPathScripts + "\\Script.txt");
                Globals.scriptPath = path;
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                }

                string formattedText="";

                foreach (SlideConfig slideConfig in Globals.SlideConfigs)
                {
                    formattedText = formattedText + slideConfig.scriptText;
                }
                File.WriteAllText(Globals.scriptPath, formattedText);
            }
            catch
            {

            }
        }

         int CountLines(string text)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(text))
            {
                count = text.Length - text.Replace("\n", string.Empty).Length;

                // if the last char of the string is not a newline, make sure to count that line too
                if (text[text.Length - 1] != '\n')
                {
                    ++count;
                }
            }

            return count;
        }

    }
}
