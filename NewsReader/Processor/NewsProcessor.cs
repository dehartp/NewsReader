using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsReader.Model;

namespace NewsReader.Processor
{
    public class NewsProcessor : INewsProcessor
    {
        private const string FireBaseUri = "https://hacker-news.firebaseio.com/v0/";
        private const int PageSize = 10;
        private readonly IDataRetrieval _dataRetrieval;
        private readonly Dictionary<string, IStory> _stories;
        private List<string> _storyIds;

        public NewsProcessor(IDataRetrieval dataRetrieval)
        {
            _dataRetrieval = dataRetrieval;
            _stories = new Dictionary<string, IStory>();
            LoadNewestStories();
        }

        public int NumberOfPages =>
            _storyIds.Count % PageSize == 0 ? _storyIds.Count / PageSize : _storyIds.Count / PageSize + 1;

        public IEnumerable<IStory> GetStoryListPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > NumberOfPages)
            {
                throw new InvalidOperationException($"Invalid page number: {pageNumber}");
            }

            var results = new List<IStory>();
            var storiesToReturn = _storyIds.GetRange(GetPageStartIndex(pageNumber), GetRangeCount(pageNumber));

            foreach (var storyId in storiesToReturn) results.Add(GetStory(storyId));

            return results;
        }

        private IStory GetStory(string storyId)
        {
            if (!_stories.ContainsKey(storyId)) _stories.Add(storyId, LoadStory(storyId));

            return _stories[storyId];
        }

        private IStory LoadStory(string storyId)
        {
            return _dataRetrieval.GetStory(storyId);
        }

        private void LoadNewestStories()
        {
            _storyIds = _dataRetrieval.GetListOfStories().ToList();
        }

        private int GetPageStartIndex(int pageNumber)
        {
            return (pageNumber - 1) * PageSize;
        }

        private int GetRangeCount(int pageNumber)
        {
            return pageNumber < NumberOfPages ? PageSize : _storyIds.Count % PageSize;
        }
    }
}
