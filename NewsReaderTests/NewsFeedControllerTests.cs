using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NewsReader.Controllers;
using NewsReader.Model;
using NewsReader.Processor;
using NUnit.Framework;

namespace NewsReaderTests
{
    [TestFixture]
    public class NewsFeedControllerTests
    {
        private Mock<INewsProcessor> _newsProcessorMock;

        [SetUp]
        public void Setup()
        {
            _newsProcessorMock = new Mock<INewsProcessor>();
        }

        [Test]
        public void NewsFeedController_Get_Stories_CallsProcessor_CorrectPageNumber()
        {
            var pageNumber = 3;
            var newsFeedController = new NewsFeedController(_newsProcessorMock.Object);

            newsFeedController.Get(pageNumber);

            _newsProcessorMock.Verify(m => m.GetPageOfStories(pageNumber, null), Times.Once);
        }

        [Test]
        public void NewsFeedController_Get_Stories_ReturnsFromProcessor()
        {
            var pageNumber = 3;
            var list = new List<Story>();
            _newsProcessorMock.Setup(m => m.GetPageOfStories(pageNumber, null)).Returns(list);
            var newsFeedController = new NewsFeedController(_newsProcessorMock.Object);

            var results = newsFeedController.Get(pageNumber);

            Assert.AreSame(list, results);
        }

        [Test]
        public void NewsFeedController_Get_NumberOfPages_CallsProcessor()
        {
            var newsFeedController = new NewsFeedController(_newsProcessorMock.Object);

            newsFeedController.Get();

            _newsProcessorMock.Verify(m => m.GetNumberOfPages(null), Times.Once);
        }

        [Test]
        public void NewsFeedController_Get_NumberOfPages_ReturnsValueFromProcessor()
        {
            var newsFeedController = new NewsFeedController(_newsProcessorMock.Object);
            _newsProcessorMock.Setup(m => m.GetNumberOfPages(null)).Returns(2);

            var result = newsFeedController.Get();

            Assert.AreEqual(2, result);
        }
    }
}
