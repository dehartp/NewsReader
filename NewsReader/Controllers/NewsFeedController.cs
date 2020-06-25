using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsReader.Model;
using NewsReader.Processor;

namespace NewsReader.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsFeedController : ControllerBase
    {
        private readonly INewsProcessor _newsProcessor;

        public NewsFeedController(INewsProcessor newsProcessor)
        {
            _newsProcessor = newsProcessor;
        }
        
        [HttpGet("stories/{pageNumber}")]
        public IEnumerable<Story> Get(int pageNumber)
        {
            return _newsProcessor.GetPageOfStories(pageNumber);
        }

        [HttpGet("pages")]
        public int Get()
        {
            return _newsProcessor.GetNumberOfPages();
        }

        [HttpGet("stories/search/{pageNumber}/{searchTerm}")]
        public IEnumerable<Story> Get(int pageNumber, string searchTerm)
        {
            return _newsProcessor.GetPageOfStories(pageNumber, searchTerm);
        }

        [HttpGet("pages/search/{searchTerm}")]
        public int Get(string searchTerm)
        {
            return _newsProcessor.GetNumberOfPages(searchTerm);
        }
    }
}
