using System;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.Extensions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    internal sealed class TrimmingConverterTests
    {
        private TrimmingConverter converter;

        private Mock<JsonReader> reader;
        private string readerValue;

        [SetUp]
        public void Setup()
        {
            converter = new TrimmingConverter();
            reader = new Mock<JsonReader>();
            readerValue = "   a string    ";
            reader.Setup(r => r.Value).Returns(() => readerValue);
        }

        [Test]
        public void ConverterCanOnlyRead()
        {
            converter.CanRead.Should().BeTrue();
            converter.CanWrite.Should().BeFalse();
        }

        [Test]
        public void WriteJsonThrowsNotImplementedException()
        {
            Assert.Throws<NotSupportedException>(() => converter.WriteJson(null, null, null));
        }

        [Test]
        public void CanConvertReturnsTrueForStrings()
        {
            converter.CanConvert(typeof(string)).Should().BeTrue();
        }

        [Test]
        public void CanConvertReturnsFalseForNonStrings()
        {
            converter.CanConvert(typeof(int)).Should().BeFalse();
            converter.CanConvert(typeof(long)).Should().BeFalse();
            converter.CanConvert(typeof(object)).Should().BeFalse();
            converter.CanConvert(typeof(TrimmingConverter)).Should().BeFalse();
        }

        [Test]
        public void ReadJsonReturnsTrimmedValueFromReader()
        {
            var returned = converter.ReadJson(reader.Object, typeof(string), null, null);
            returned.Should().BeOfType<string>();
            returned.Should().Be("a string");
        }

        [Test]
        public void ReadJsonReturnsNullFromReader()
        {
            readerValue = null;
            var returned = converter.ReadJson(reader.Object, typeof(string), null, null);
            returned.Should().BeNull();
        }

        [Test]
        public void ReadJsonReturnsNullFromNullReader()
        {
            var returned = converter.ReadJson(null, typeof(string), null, null);
            returned.Should().BeNull();
        }
    }
}
