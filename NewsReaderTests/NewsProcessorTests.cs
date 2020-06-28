using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NewsReader.Model;
using NewsReader.Processor;
using NUnit.Framework;

namespace NewsReaderTests
{
    [TestFixture]
    public class NewsProcessorTests
    {
        private const int PageSize = 10;
        private Mock<IDataRetrieval> _dataRetrievalMock;

        [SetUp]
        public void Setup()
        {
            _dataRetrievalMock = new Mock<IDataRetrieval>();
        }

        [Test]
        public void NewsProcessor_Constructor_Calls_GetListOfStories()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);

            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            _dataRetrievalMock.Verify(m => m.GetListOfStories(), Times.Once);
        }

        [Test]
        public void NewsProcessor_Constructor_Calls_GetStory_CorrectNumberOfTimes()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);

            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            _dataRetrievalMock.Verify(m => m.GetStory("1"), Times.Once);
            _dataRetrievalMock.Verify(m => m.GetStory(It.IsAny<string>()), Times.Exactly(25));
        }

        [Test]
        public void NewsProcessor_GetNumberOfPages_PartialPage_CorrectValue()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var result = newsProcessor.GetNumberOfPages();
 
            Assert.AreEqual(3, result);
        }

        [Test]
        public void NewsProcessor_GetNumberOfPages_NoPartialPage_CorrectValue()
        {
            var numberOfStories = 30;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var result = newsProcessor.GetNumberOfPages();

            Assert.AreEqual(3, result);
        }

        [Test]
        public void NewsProcessor_GetNumberOfPages_Search_CorrectValue()
        {
            var numberOfStories = 30;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var result = newsProcessor.GetNumberOfPages("4");

            // 3 records expected to match
            Assert.AreEqual(1, result);
        }

        [Test]
        public void NewsProcessor_GetNumberOfPages_Search_NoResults_Returns0()
        {
            var numberOfStories = 30;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var result = newsProcessor.GetNumberOfPages("44");

            // 0 records expected to match
            Assert.AreEqual(0, result);
        }

        [Test]
        public void NewsProcessor_GetPageOfStories_FullPage_ReturnsCorrectStories()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var results = newsProcessor.GetPageOfStories(2).ToList();

            Assert.AreEqual(PageSize, results.Count());

            // Expect stories 15 through 6
            for (var i = 0; i < PageSize; i++)
            {
                var story = results[i];
                Assert.AreEqual((15 - i).ToString(), story.Id);
                Assert.AreEqual("Title" + (15 - i).ToString(), story.Title);
                Assert.AreEqual("Url" + (15 - i).ToString(), story.Url);
            }
        }

        [Test]
        public void NewsProcessor_GetPageOfStories_PartialPage_ReturnsCorrectStories()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            var results = newsProcessor.GetPageOfStories(3).ToList();

            Assert.AreEqual(5, results.Count());

            // Expect stories 5 through 1
            for (var i = 0; i < 5 ; i++)
            {
                var story = results[i];
                Assert.AreEqual((5 - i).ToString(), story.Id);
                Assert.AreEqual("Title" + (5 - i).ToString(), story.Title);
                Assert.AreEqual("Url" + (5 - i).ToString(), story.Url);
            }
        }

        [Test]
        public void NewsProcessor_GetPageOfStories_UsesCachedValue()
        {
            var numberOfStories = 25;
            SetupTestData(numberOfStories);
            var newsProcessor = new NewsProcessor(_dataRetrievalMock.Object);

            _dataRetrievalMock.Verify(m => m.GetStory("25"), Times.Once);

            var results = newsProcessor.GetPageOfStories(1).ToList();

            var story = results[0];
            Assert.AreEqual("25", story.Id);
            _dataRetrievalMock.Verify(m => m.GetStory("25"), Times.Once);
        }

        private void SetupTestData(int numberOfTestStories)
        {
            var storyIdList = new List<string>();
            for (var i = 1; i <= numberOfTestStories; i++)
            {
                var index = i.ToString(); // avoid issues with capture
                storyIdList.Add(index);
                _dataRetrievalMock.Setup(m => m.GetStory(index))
                    .Returns(new Story {Title = "Title" + index, Url = "Url" + index, Id = index});
            }

            _dataRetrievalMock.Setup(m => m.GetListOfStories()).Returns(storyIdList);
        }
    }
}
