using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PrivateCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Hostings
{
    [TestFixture]
    public sealed class PrivateCloudHostingControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private PrivateCloudHostingController _controller;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PrivateCloudHostingController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdatePrivateCloudCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }


        [TestCase("Some summary", "Some link", "Some hosting model", "'Tis required")]
        [TestCase("Some summary", null, "Some hosting model", "'Tis required")]
        [TestCase(null, "Some link", "Some hosting model", "'Tis required")]
        [TestCase("Some summary", "Some link", "Some hosting model", null)]
        [TestCase("Some summary", "Some link", null, "'Tis required")]
        [TestCase(null, null, "Some hosting model", "'Tis required")]
        [TestCase("Some summary", null, "Some hosting model", null)]
        [TestCase(null, "Some link", "Some hosting model", null)]
        [TestCase("Some summary", null, null, "'Tis required")]
        [TestCase(null, "Some link", null, "'Tis required")]
        [TestCase("Some summary", "Some link", null, null)]
        [TestCase(null, null, "Some hosting model", null)]
        [TestCase(null, null, null, "'Tis required")]
        [TestCase("Some summary", null, null, null)]
        [TestCase(null, "Some link", null, null)]
        [TestCase(null, null, null, null)]
        public async Task PopulatedPrivateCloudShouldReturnCorrectDetails(string summary, string link, string hostingModel, string requiredHscn)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.PrivateCloud == Mock.Of<IPrivateCloud>(p =>
                                                         p.Summary == summary &&
                                                         p.Link == link &&
                                                         p.HostingModel == hostingModel &&
                                                         p.RequiresHSCN == requiredHscn)));
            var result = await _controller.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetPrivateCloudResult>();

            var privateCloudResult = result.Value as GetPrivateCloudResult;
            privateCloudResult.Summary.Should().Be(summary);
            privateCloudResult.Link.Should().Be(link);
            privateCloudResult.HostingModel.Should().Be(hostingModel);

            if (requiredHscn == null)
            {
                privateCloudResult.RequiredHscn.Should().BeEmpty();
            }
            else
            {
                privateCloudResult.RequiredHscn.Should().BeEquivalentTo(requiredHscn);
            }
        }

        [Test]
        public async Task NullHostingShouldReturnNull()
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IHosting);

            var result = await _controller.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetPrivateCloudResult>();

            var privateCloudResult = result.Value as GetPrivateCloudResult;
            privateCloudResult.Summary.Should().BeNull();
            privateCloudResult.Link.Should().BeNull();
            privateCloudResult.HostingModel.Should().BeNull();
            privateCloudResult.RequiredHscn.Should().BeEmpty();
        }

        [Test]
        public async Task UpdatePrivateCloudDetails()
        {
            var request = new UpdatePrivateCloudViewModel();

            var result =
                (await _controller.Update(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdatePrivateCloudCommand>(c =>
                        c.Id == _solutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("summary", "maxLength");
            _resultDictionary.Add("link", "maxLength");
            _resultDictionary.Add("hosting-model", "maxLength");

            var request = new UpdatePrivateCloudViewModel();

            var result =
                (await _controller.Update(_solutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(3);
            validationResult["summary"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");
            validationResult["hosting-model"].Should().Be("maxLength");

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdatePrivateCloudCommand>(c =>
                        c.Id == _solutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

