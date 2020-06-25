using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsReader.Model
{
    public class Story : IStory
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
