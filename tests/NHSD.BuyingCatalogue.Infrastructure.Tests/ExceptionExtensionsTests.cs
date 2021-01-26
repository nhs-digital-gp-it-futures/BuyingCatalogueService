using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Infrastructure.Tests
{
    [TestFixture]
    internal sealed class ExceptionExtensionsTests
    {
        [Test]
        public void ShouldReturnStatusCode404()
        {
            var exception = new NotFoundException();
            exception.ToStatusCode().Should().Be(404);
        }

        [Test]
        [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Extension method test")]
        public void ShouldReturnStatusCode500()
        {
            var exception = new Exception();
            exception.ToStatusCode().Should().Be(500);
        }

        [Test]
        [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Extension method test")]
        public void ShouldMapToJsonMessageVerboseFalse()
        {
            var exception = new Exception();
            var res = exception.ToJsonMessage(false);

            var json = JToken.Parse(JsonConvert.SerializeObject(res.Value));

            json.SelectToken("errors").First().Value<string>().Should().Be("An unexpected error occurred.");
            json.SelectToken("detail").Value<string>().Should().Be(exception.Message);
        }

        [Test]
        [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Extension method test")]
        public void ShouldMapToJsonMessagesVerboseTrue()
        {
            var exception = new Exception();

            var res = exception.ToJsonMessage(true);

            var json = JToken.Parse(JsonConvert.SerializeObject(res.Value));

            json.SelectToken("errors").First().Value<string>().Should().Be("An unexpected error occurred.");
            json.SelectToken("detail").Value<string>().Should().Be(exception.ToString());
        }

        [Test]
        public void NullExceptionToJsonMessageShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ((Exception)null).ToJsonMessage(false));
        }
    }
}
