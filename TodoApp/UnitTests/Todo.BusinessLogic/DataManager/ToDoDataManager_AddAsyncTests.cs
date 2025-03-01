using BusinessLogic.Blas;
using BusinessLogic.DataManager;
using BusinessLogic.Enums;
using BusinessLogic.SequenceGenerator;
using DataLayer.Daos;
using Moq;
using NUnit.Framework;

namespace UnitTests.Todo.BusinessLogic.DataManager
{
    //Each method should have its own test class (This methodology does create a lot of classes, but it is easier to maintain in the long run)
    [TestFixture]
    internal class ToDoDataManager_AddAsyncTests //class_MethodToTest
    {
        private ToDoDataManager _toDoDataManager;
        private Mock<ICosmosDataAccessManager<ToDoDao>> _dataAccessMock;
        private Mock<ISequenceGenerator> _sequenceGeneratorMock;
        [SetUp]
        public void Setup()
        {
            _dataAccessMock = new Mock<ICosmosDataAccessManager<ToDoDao>>(MockBehavior.Strict);
            _sequenceGeneratorMock = new Mock<ISequenceGenerator>(MockBehavior.Strict);
            _toDoDataManager = new ToDoDataManager(_dataAccessMock.Object, _sequenceGeneratorMock.Object);
        }

        [Test(Description = "Jira ticket-number and title")]
        public async Task GivenIdIsSetTo1_CreatesNewRecord()
        {
            // Arrange
            var id = "1";
            var request = new ToDoPostRequestBodyBla("title", "description", StatusBlaEnum.NotStarted);
            _sequenceGeneratorMock.Setup(x => x.GetNext()).Returns(1);
            _dataAccessMock.Setup(x => x.AddItemAsync(It.Is<ToDoDao>(y => y.Id == id))).Returns(Task.CompletedTask);
            // Act
            var result = await _toDoDataManager.AddAsync(request);
            // Assert
            Assert.That(id, Is.EqualTo(result));
            _sequenceGeneratorMock.Verify(x => x.GetNext(), Times.Once);
            _dataAccessMock.Verify(x => x.AddItemAsync(It.Is<ToDoDao>(y => y.Id == id)), Times.Once);
        }
    }
}
