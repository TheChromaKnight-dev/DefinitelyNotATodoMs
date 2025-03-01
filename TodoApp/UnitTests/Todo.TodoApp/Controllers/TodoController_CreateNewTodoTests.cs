using BusinessLogic.Blas;
using BusinessLogic.Services;
using CodeGen;
using Moq;
using NUnit.Framework;
using TodoApp.Controllers;

namespace UnitTests.Todo.TodoApp.Controllers
{
    [TestFixture]
    internal class TodoController_CreateNewTodoTests
    {
        private ToDoController _controller;
        private Mock<IToDoService> _service;
        [SetUp]
        public void Setup()
        {
            _service = new Mock<IToDoService>(MockBehavior.Strict);
            _controller = new ToDoController(_service.Object);
        }

        [Test(Description = "jira ticket")]
        public async Task RequestBodyIsFilled_ReturnsGeneratedId()
        {
            // Arrange
            var request = new ToDoPostRequestBody("Title", "Description", StatusEnum.NotStarted);
            var id = "1";
            _service.Setup(x => x.CreateNewTodo(It.IsAny<ToDoPostRequestBodyBla>())).ReturnsAsync(id);

            // Act
            var result = await _controller.CreateNewTodo(request);

            // Assert
            Assert.That(result.id, Is.EqualTo(id));
            _service.Verify(x => x.CreateNewTodo(It.IsAny<ToDoPostRequestBodyBla>()), Times.Once);
        }

    }
}
