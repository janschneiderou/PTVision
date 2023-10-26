using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.LogObjects
{
    public class VocalExerciseSession
    {
        public DateTime start;
        public string excercise="";

        public VocalExerciseSession()
        {
            start = DateTime.Now;
        }

    }
}
