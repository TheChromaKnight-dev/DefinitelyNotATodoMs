using System.Collections.Immutable;
using BusinessLogic.Blas;
using BusinessLogic.Enums;
using BusinessLogic.Services;
using CodeGen;
using Moq;
using NUnit.Framework;
using TodoApp.Controllers;
using TodoApp.DtoToBlaMappers;

namespace UnitTests.Todo.TodoApp.Controllers
{
    [TestFixture]
    internal class TodoController_GetAllTodosTests
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
        public async Task DescriptionIsNull_ReturnsAllTodos()
        {
            // Arrange
            string description = null;
            string firstId = "1";
            string secondId = "1";
            var todos = ImmutableArray.Create(new ToDoEntityBla(firstId, "Title1", "Description1", StatusBlaEnum.NotStarted),
                    new ToDoEntityBla(secondId, "Title2", "Description2", StatusBlaEnum.Completed));
            _service.Setup(x => x.GetAllAsync(description)).ReturnsAsync(todos);

            // Act
            var result = await _controller.GetAllTodos(description);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(firstId));
            Assert.That(result[1].Id, Is.EqualTo(secondId));
            _service.Verify(x => x.GetAllAsync(description), Times.Once);
        }
    }
}
