using BusinessLogic.Enums;
using CodeGen;
using NUnit.Framework;
using TodoApp.DtoToBlaMappers;

namespace UnitTests.Todo.TodoApp.DtoToBlaMappers
{
    [TestFixture]
    internal class StatusBlaEnumToDtoMapper_MapToDtoTests
    {
        [TestCase(StatusBlaEnum.Completed, StatusEnum.Completed), Description("jiraTicket")]
        [TestCase(StatusBlaEnum.NotStarted, StatusEnum.NotStarted)]
        public void BlaToDto_StatusBlaEnumCompleted_ReturnsStatusEnumCompleted(StatusBlaEnum actual, StatusEnum expected)
        {
            // Act
            var result = actual.MapToDto();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test(Description = "jiraTicket")]
        public void BlaToDto_DefaultShouldThrowException()
        {
            // Act
            Assert.Throws<NotImplementedException>(() => ((StatusBlaEnum)3).MapToDto());
        }

        [TestCase(StatusEnum.Completed, StatusBlaEnum.Completed), Description("jiraTicket")]
        [TestCase(StatusEnum.NotStarted, StatusBlaEnum.NotStarted)]
        public void DtoToBla_EnumPropertiesShouldMapCorrectly(StatusEnum actual, StatusBlaEnum expected)
        {
            // Act
            var result = actual.MapToBla();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
        [Test(Description = "jiraTicket")]
        public void DtoToBla_DefaultShouldThrowException()
        {
            // Act
            Assert.Throws<NotImplementedException>(() => ((StatusEnum)3).MapToBla());
        }
    }
}
