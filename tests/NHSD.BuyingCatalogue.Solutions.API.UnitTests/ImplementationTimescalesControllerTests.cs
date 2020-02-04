using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class ImplementationTimescalesControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private ImplementationTimescalesController _controller;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ImplementationTimescalesController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
        }

        [TestCase("Some implementation timescales description")]
        [TestCase(null)]
        public async Task ShouldGetImplementationTimescales(string description)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetImplementationTimescalesBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IImplementationTimescales>(m => m.Description == description));

            var result = await _controller.Get(SolutionId).ConfigureAwait(false);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;
            objectResult.Value.Should().BeOfType<ImplementationTimescalesResult>();

            var implementationTimescalesResult = objectResult.Value as ImplementationTimescalesResult;
            implementationTimescalesResult.Should().NotBeNull();
            implementationTimescalesResult.Description.Should().Be(description);

            _mediatorMock.Verify(m => m.Send(It.Is<GetImplementationTimescalesBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}
