using System;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.Extensions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public class TrimmingConverterTests
    {
        private TrimmingConverter _converter;

        private Mock<JsonReader> _reader;
        private string _readerValue;

        [SetUp]
        public void Setup()
        {
            _converter = new TrimmingConverter();
            _reader = new Mock<JsonReader>();
            _readerValue = "   a string    ";
            _reader.Setup(x => x.Value).Returns(() => _readerValue);
        }

        [Test]
        public void ConverterCanOnlyRead()
        {
            _converter.CanRead.Should().BeTrue();
            _converter.CanWrite.Should().BeFalse();
        }

        [Test]
        public void WriteJsonThrowsNotImplementedException()
        {
            Assert.Throws<NotSupportedException>(() => _converter.WriteJson(null, null, null));
        }

        [Test]
        public void CanConvertReturnsTrueForStrings()
        {
            _converter.CanConvert(typeof(string)).Should().BeTrue();
        }

        [Test]
        public void CanConvertReturnsFalseForNonStrings()
        {
            _converter.CanConvert(typeof(int)).Should().BeFalse();
            _converter.CanConvert(typeof(long)).Should().BeFalse();
            _converter.CanConvert(typeof(object)).Should().BeFalse();
            _converter.CanConvert(typeof(TrimmingConverter)).Should().BeFalse();
        }

        [Test]
        public void ReadJsonReturnsTrimmedValueFromReader()
        {
            var returned = _converter.ReadJson(_reader.Object, typeof(string), null, null);
            returned.Should().BeOfType<string>();
            returned.Should().Be("a string");
        }

        [Test]
        public void ReadJsonReturnsNullFromReader()
        {
            _readerValue = null;
            var returned = _converter.ReadJson(_reader.Object, typeof(string), null, null);
            returned.Should().BeNull();
        }

        [Test]
        public void ReadJsonReturnsNullFromNullReader()
        {
            var returned = _converter.ReadJson(null, typeof(string), null, null);
            returned.Should().BeNull();
        }
    }
}
