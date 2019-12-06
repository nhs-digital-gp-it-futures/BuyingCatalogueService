using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    public class NotFoundExceptionTests
    {
        private readonly string _expectedMessage = "ABC";
        private readonly Exception _innerException = new Exception();

        [Test]
        public void PassingMessageSetsExceptionMessage()
        {
            var exception = new NotFoundException(_expectedMessage);
            exception.Message.Should().Be(_expectedMessage);
        }

        [Test]
        public void PassingInnerExceptionSetsExceptionInner()
        {
            var exception = new NotFoundException(_expectedMessage, _innerException);
            exception.Message.Should().Be(_expectedMessage);
            exception.InnerException.Should().Be(_innerException);
        }

        [Test]
        public void PassingNameAndKeyPopulatesMessage()
        {
            var exception = new NotFoundException("MyName", "MyKey");
            exception.Message.Should().Be("Entity named 'MyName' could not be found matching the ID 'MyKey'.");
        }
    }
}
