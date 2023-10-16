﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.LogObjects
{
    public  class MemorySession
    {
        public DateTime start;
        public int level;
        public List <IdentifiedSentence> sentences;

        public MemorySession()
        {
            sentences = new List <IdentifiedSentence>();
        }

    }
}
