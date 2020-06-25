using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsReader.Model
{
    public interface IStory
    {
        string Title { get; set; }

        string Url { get; set; }
    }
}
