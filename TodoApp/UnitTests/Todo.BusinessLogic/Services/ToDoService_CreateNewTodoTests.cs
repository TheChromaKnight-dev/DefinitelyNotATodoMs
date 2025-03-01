using BusinessLogic.Blas;
using BusinessLogic.DataManager;
using BusinessLogic.Enums;
using BusinessLogic.Services;
using Moq;
using NUnit.Framework;

namespace UnitTests.Todo.BusinessLogic.Services
{
    [TestFixture]
    internal class ToDoService_CreateNewTodoTests
    {
        private ToDoService _toDoService;
        private Mock<IToDoDataManager> _toDoDataManagerMock;
        [SetUp]
        public void Setup()
        {
            _toDoDataManagerMock = new Mock<IToDoDataManager>(MockBehavior.Strict);
            _toDoService = new ToDoService(_toDoDataManagerMock.Object);
        }

        [Test(Description = "jira ticket")]
        public async Task ShouldReturnGeneratedId()
        {
            // Arrange
            string id = "1";
            var request = new ToDoPostRequestBodyBla("title", "description", StatusBlaEnum.NotStarted);
            _toDoDataManagerMock.Setup(x => x.AddAsync(It.IsAny<ToDoPostRequestBodyBla>())).ReturnsAsync(id);

            // Act
            var result = await _toDoService.CreateNewTodo(request);

            // Assert
            Assert.That(result, Is.EqualTo(id));
            _toDoDataManagerMock.Verify(x => x.AddAsync(It.IsAny<ToDoPostRequestBodyBla>()), Times.Once);
        }


    }
}
