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
    /// Interaction logic for OutroComposition.xaml
    /// </summary>
    public partial class OutroComposition : UserControl
    {
        public OutroComposition()
        {
            InitializeComponent();
        }
        public void loadContent()
        {
            string content = "";
            content = content + topic() + "\n\n\n";
            content = content + before() +"\n\n\n";
            content = content + intro() + "\n\n\n";
            content = content + middle() + "\n\n\n";
            content = content + conclusion() + "\n\n\n";
            content = content + after();
            structureText.Text = content;
        }

        string topic()
        {
            return "Topic:" + Globals.MessageStructure.presentationTopic;
        }
        string before()
        {
            string s="Previous Knowledge:\n";
            foreach (string t in Globals.MessageStructure.audiencePrevious)
            {
                s += t+"\n";
            }
            return s;
        }
        string intro()
        {
            string s = "\nIntroduction:\n\n";
            foreach(IntroductionStarters i in Globals.MessageStructure.introductionStarters)
            {
                s += i.starter + ":\n";
                foreach(string t in i.pointers)
                {
                    s += t+"\n";
                }
            }

            return s;
        }
        string middle()
        {
            string s = "\nMiddle:\n\n";
            foreach (string t in Globals.MessageStructure.middleStatements)
            {
                s += t + "\n";
            }
            return s;
        }
        string conclusion()
        {
            string s = "\nConclusion:\n";
            s = s + "Open Brackets:\n";
            foreach (string b in Globals.MessageStructure.conclusionLogs.openBrackets)
            {
                s += b + "\n";
            }
            s = s + "Final Message:\n";
            foreach (string b in Globals.MessageStructure.conclusionLogs.finalMessage)
            {
                s += b + "\n";
            }
            return s;
        }
        string after()
        {
            string s = "Knowledge after presentation:\n";
            foreach (string t in Globals.MessageStructure.audienceAfter)
            {
                s += t + "\n";
            }

            return s;
        }
    }
}
