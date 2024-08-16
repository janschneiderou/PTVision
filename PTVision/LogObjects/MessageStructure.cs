using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.LogObjects
{
    public class MessageStructure
    {
        public  string presentationTopic = "";
        public List<string> audiencePrevious = new List<string>();
        public List<string> audienceAfter = new List<string>();
        public List<string> middleStatements = new List<string>();
        public List<IntroductionStarters> introductionStarters = new List<IntroductionStarters>();
        public ConclusionLogs conclusionLogs = new ConclusionLogs();
        public MessageStructure() 
        { 
        }
       
    }
}
