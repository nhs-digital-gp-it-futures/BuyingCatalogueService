using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeMobile
{
    [TestFixture]
    public sealed class HardwareRequirementsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private HardwareRequirementsController _controller;
        private readonly string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new HardwareRequirementsController(_mediatorMock.Object);
        }

        [TestCase("New Hardware")]
        [TestCase("       ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task PopulatedHardwareDetailsShouldReturnHardwareDetails(string hardwareRequirements)
        {
            _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.NativeMobileHardwareRequirements == hardwareRequirements));

            var result = await _controller.GetHardwareRequirements(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetHardwareRequirementsResult>();
            var hardwareResult = result.Value as GetHardwareRequirementsResult;
            hardwareResult.HardwareRequirements.Should().Be(hardwareRequirements);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = (await _controller.GetHardwareRequirements(_solutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetHardwareRequirementsResult>();
            var hardwareResult = result.Value as GetHardwareRequirementsResult;
            hardwareResult.HardwareRequirements.Should().BeNull();
        }
    }
}
