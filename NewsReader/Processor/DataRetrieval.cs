using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NewsReader.Model;
using Newtonsoft.Json;

namespace NewsReader.Processor
{
    public class DataRetrieval : IDataRetrieval
    {
        private const string BaseUri = "https://hacker-news.firebaseio.com/v0/";
        private readonly IHttpClientFactory _clientFactory;

        public DataRetrieval(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IList<string> GetListOfStories()
        {
            var client = _clientFactory.CreateClient();

            var responseTask = client.GetAsync(BaseUri + "newstories.json");

            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<string>>(result.Content.ReadAsStringAsync().Result);

            throw new ApplicationException("Error calling hacker news.  Status code: " + result.StatusCode);
        }

        public Story GetStory(string storyId)
        {
            var client = _clientFactory.CreateClient();

            var responseTask = client.GetAsync(new Uri(BaseUri + "item/" + storyId + ".json"));

            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Story>(result.Content.ReadAsStringAsync().Result);

            throw new ApplicationException("Error calling hacker news.  Status code: " + result.StatusCode);
        }
    }
}
