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
        private List<Story> _stories;

        public NewsProcessor(IDataRetrieval dataRetrieval)
        {
            _dataRetrieval = dataRetrieval;
            _stories = new List<Story>();
            LoadNewestStories();
        }

        public IEnumerable<Story> GetPageOfStories(int pageNumber, string searchTerm = null)
        {
            var numberOfPages = GetNumberOfPages(searchTerm);

            if (numberOfPages == 0)
            {
                return new List<Story>();
            }

            if (pageNumber < 1 || pageNumber > numberOfPages)
            {
                throw new InvalidOperationException($"Invalid page number: {pageNumber} for search term {searchTerm}");
            }

            var searchedList = GetSearchedListOfStories(searchTerm);

            return searchedList.GetRange(GetPageStartIndex(pageNumber), GetRangeCount(pageNumber, numberOfPages, searchedList.Count));
        }

        public int GetNumberOfPages(string searchTerm = null)
        {
            var searchedList = GetSearchedListOfStories(searchTerm);

            return searchedList.Count() % PageSize == 0 ? (searchedList.Count() / PageSize) : (searchedList.Count() / PageSize) + 1;
        }

        private List<Story> GetSearchedListOfStories(string searchTerm)
        {
            return string.IsNullOrEmpty(searchTerm) ? _stories : _stories.Where(s => s.Title.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        private Story LoadStory(string storyId)
        {
            return _dataRetrieval.GetStory(storyId);
        }

        private void LoadNewestStories()
        {
            var stories = new List<Story>();
            var storyIds = _dataRetrieval.GetListOfStories();
            foreach (var storyId in storyIds)
            {
                var story = LoadStory(storyId);

                if (story != null && !string.IsNullOrEmpty(story.Url))
                {
                    stories.Add(story);
                }
            }

            // Newest stories first; hacker news specification ids are integers
            _stories = stories.OrderByDescending(s => int.Parse(s.Id)).ToList();
        }

        private int GetPageStartIndex(int pageNumber)
        {
            return (pageNumber - 1) * PageSize;
        }

        private int GetRangeCount(int pageNumber, int numberOfPages, int listCount)
        {
            return pageNumber < numberOfPages ? PageSize : listCount % PageSize;
        }
    }
}
