using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsReader.Model;

namespace NewsReader.Processor
{
    public interface IDataRetrieval
    {
        IList<string> GetListOfStories();

        Story GetStory(string storyId);
    }
}
