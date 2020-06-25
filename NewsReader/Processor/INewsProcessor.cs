using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsReader.Model;

namespace NewsReader.Processor
{
    public interface INewsProcessor
    {
        IEnumerable<IStory> GetStoryListPage(int pageNumber);

        int NumberOfPages { get; }
    }
}
