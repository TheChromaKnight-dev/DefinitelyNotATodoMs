using System.Runtime.ExceptionServices;
using BusinessLogic.DataManager;
using BusinessLogic.SequenceGenerator;
using DataLayer.Daos;
using Moq;
using NUnit.Framework;

namespace UnitTests.Todo.BusinessLogic.DataManager
{
    [TestFixture]
    internal class ToDoDataManager_GetAllAsyncTests
    {
        private ToDoDataManager _toDoDataManager;
        private Mock<ICosmosDataAccessManager<ToDoDao>> _dataAccessMock;
        [SetUp]
        public void Setup()
        {
            _dataAccessMock = new Mock<ICosmosDataAccessManager<ToDoDao>>(MockBehavior.Strict);
            _toDoDataManager = new ToDoDataManager(_dataAccessMock.Object, null!);
        }

        [Test(Description = "jira ticket")]
        public async Task DescriptionIsEmpty_2ItemsAreIntheDb_ShouldReturnAllItems()
        {
            //Arrange
            string firstId = "1";
            string secondId = "2";
            _dataAccessMock.Setup(x => x.QueryItemsAsync(It.IsAny<string>())).ReturnsAsync([
                new ToDoDao
                {
                    Id = firstId,
                    Title = "title",
                    Description = "description",
                    Status = "NotStarted"
                },

                new ToDoDao
                {
                    Id = secondId,
                    Title = "title",
                    Description = "description",
                    Status = "NotStarted"
                }
            ]);
            //Act
            var res = await _toDoDataManager.GetAllAsync(null);

            //Assert
            Assert.That(res[0].Id, Is.EqualTo(firstId));
            Assert.That(res[1].Id, Is.EqualTo(secondId));
            _dataAccessMock.Verify(x => x.QueryItemsAsync(It.Is<string>(y
                => y == "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo")), Times.Once);
        }
        [Test(Description = "jira ticket")]
        public async Task DbIsEmpty_ShouldReturnEmptyCollection()
        {
            //Arrange
            string firstId = "1";
            string secondId = "2";
            _dataAccessMock.Setup(x => x.QueryItemsAsync(It.IsAny<string>())).ReturnsAsync([
            ]);
            //Act
            var res = await _toDoDataManager.GetAllAsync(null);

            //Assert
            Assert.That(res.IsEmpty, Is.True);
            
            _dataAccessMock.Verify(x => x.QueryItemsAsync(It.Is<string>(y
                => y == "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo")), Times.Once);
        }
        [Test(Description = "jira ticket")]
        public async Task DescriptionIsSet_2ItemsAreIntheDb_ShouldFilterByDescription()
        {
            //Arrange
            string firstId = "1";
            string secondId = "2";
            _dataAccessMock.Setup(x => x.QueryItemsAsync(It.IsAny<string>())).ReturnsAsync([
                new ToDoDao
                {
                    Id = firstId,
                    Title = "title",
                    Description = "text",
                    Status = "NotStarted"
                },

                new ToDoDao
                {
                    Id = secondId,
                    Title = "title",
                    Description = "description",
                    Status = "NotStarted"
                }
            ]);
            //Act
            var res = await _toDoDataManager.GetAllAsync("tex");

            //Assert
            Assert.That(res.Length, Is.EqualTo(1));
            Assert.That(res[0].Id, Is.EqualTo(firstId));
            _dataAccessMock.Verify(x => x.QueryItemsAsync(It.Is<string>(y
                => y == "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo")), Times.Once);
        }

        [Test(Description = "jira ticket")]
        public async Task DbIsEmpty_DescriptionIsSet_ShouldReturnEmptyCollection()
        {
            //Arrange
            string firstId = "1";
            string secondId = "2";
            _dataAccessMock.Setup(x => x.QueryItemsAsync(It.IsAny<string>())).ReturnsAsync([
            ]);
            //Act
            var res = await _toDoDataManager.GetAllAsync("text");

            //Assert
            Assert.That(res.IsEmpty, Is.True);

            _dataAccessMock.Verify(x => x.QueryItemsAsync(It.Is<string>(y
                => y == "SELECT Todo.id,Todo.Title,Todo.Description,Todo.Status FROM Table Todo")), Times.Once);
        }
    }
}
