using System;
using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    internal sealed class NotFoundExceptionTests
    {
        private const string ExpectedMessage = "ABC";
        private readonly Exception innerException = new InvalidOperationException();

        [Test]
        public void PassingMessageSetsExceptionMessage()
        {
            var exception = new NotFoundException(ExpectedMessage);
            exception.Message.Should().Be(ExpectedMessage);
        }

        [Test]
        public void PassingInnerExceptionSetsExceptionInner()
        {
            var exception = new NotFoundException(ExpectedMessage, innerException);
            exception.Message.Should().Be(ExpectedMessage);
            exception.InnerException.Should().Be(innerException);
        }

        [Test]
        public void PassingNameAndKeyPopulatesMessage()
        {
            var exception = new NotFoundException("MyName", "MyKey");
            exception.Message.Should().Be("Entity named 'MyName' could not be found matching the ID 'MyKey'.");
        }
    }
}
