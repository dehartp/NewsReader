using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsReader.Model;

namespace NewsReader.Processor
{
    public interface INewsProcessor
    {
        IEnumerable<Story> GetPageOfStories(int pageNumber, string searchTerm = null);

        int GetNumberOfPages(string searchTerm = null);
    }
}
