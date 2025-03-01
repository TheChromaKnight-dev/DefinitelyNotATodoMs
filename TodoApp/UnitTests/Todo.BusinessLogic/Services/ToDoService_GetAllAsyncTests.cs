using System.Collections.Immutable;
using BusinessLogic.Blas;
using BusinessLogic.DataManager;
using BusinessLogic.Enums;
using BusinessLogic.Services;
using Moq;
using NUnit.Framework;

namespace UnitTests.Todo.BusinessLogic.Services
{
    internal class ToDoService_GetAllAsyncTests
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
        public async Task TwoItemsAreReturnedFromDb_Return2Items()
        {
            // Arrange
            var description = "description";
            var firstId = "1";
            var secondId = "2";
            var toDoEntityBla = new ToDoEntityBla(firstId, "title", description, StatusBlaEnum.NotStarted);
            var toDoEntityBla2 = new ToDoEntityBla(secondId, "title", description, StatusBlaEnum.NotStarted);
            var toDoEntityBlaArray = ImmutableArray.Create(toDoEntityBla, toDoEntityBla2);
            _toDoDataManagerMock.Setup(x => x.GetAllAsync(It.IsAny<string>())).ReturnsAsync(toDoEntityBlaArray);

            // Act
            var result = await _toDoService.GetAllAsync(description);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(firstId));
            Assert.That(result[1].Id, Is.EqualTo(secondId));
            _toDoDataManagerMock.Verify(x => x.GetAllAsync(It.IsAny<string>()), Times.Once);
        }
        [Test(Description = "jira ticket")]
        public void NoItemsAreReturnedFromTheDb_ShouldThrow()
        {
            // Arrange
            var description = "description";
            var toDoEntityBlaArray = ImmutableArray<ToDoEntityBla>.Empty;
            _toDoDataManagerMock.Setup(x => x.GetAllAsync(It.IsAny<string>())).ReturnsAsync(toDoEntityBlaArray);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => _toDoService.GetAllAsync(description));

            // Assert
            Assert.That(exception!.Message, Is.EqualTo("No resources found"));
            _toDoDataManagerMock.Verify(x => x.GetAllAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
