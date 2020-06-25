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


        [HttpGet]
        public IEnumerable<IStory> Get()
        {
            return _newsProcessor.GetStoryListPage(1);
        }
    }
}
