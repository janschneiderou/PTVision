using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.LogObjects
{
    public class FeedbacksSentences
    {
        public List<FeedbackForSentences> feedbacks;

        public FeedbacksSentences()
        {
            feedbacks = new List<FeedbackForSentences>();
        }
    }
}
