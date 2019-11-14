using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.API.Extensions;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class CustomExceptionFilterTests
    {
        [Test]
        public void ShouldReturnStatusCode404()
        {
            var exception = new NotFoundException("exception", this);
            exception.ToStatusCode().Should().Be(404);
        }

        [Test]
        public void ShouldReturnStatusCode500()
        {
            var exception = new Exception();
            exception.ToStatusCode().Should().Be(500);
        }

        [Test]
        public void ShouldMapToJsonMessageVerboseFalse()
        {
            var exception = new Exception();
            var res = exception.ToJsonMessage(false);

            var json = JToken.Parse(JsonConvert.SerializeObject(res.Value));

            json.SelectToken("errors").First().Value<string>().Should().Be("An unexpected error occurred.");
            json.SelectToken("detail").Value<string>().Should().Be(exception.Message);
        }

        [Test]
        public void ShouldMapToJsonMessagesVerboseTrue()
        {
            var exception = new Exception();

            var res = exception.ToJsonMessage(true);

            var json = JToken.Parse(JsonConvert.SerializeObject(res.Value));

            json.SelectToken("errors").First().Value<string>().Should().Be("An unexpected error occurred.");
            json.SelectToken("detail").Value<string>().Should().Be(exception.ToString());
        }
    }
}
