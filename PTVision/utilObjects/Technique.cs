using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.utilObjects
{
    public class Technique
    {
        public List<string> Improvements=new List<string>();
        public string Modality = "";
        public string Name = "";
        public string Description = "";
        public string Feedback = "";
        public string Example = "";

        public Technique(List<string> improvements, string modality, string name, string description, string feedback, string example)
        {
            Improvements = improvements;
            Modality = modality;
            Name = name;
            Description = description;
            Feedback = feedback;
            Example = example;
        }

        public Technique()
        {
            Improvements = new List<string>();
        }
    }
}
