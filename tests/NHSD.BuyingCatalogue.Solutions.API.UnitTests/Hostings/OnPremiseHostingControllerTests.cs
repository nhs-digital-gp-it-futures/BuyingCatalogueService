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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Hostings
{
    [TestFixture]
    public sealed class OnPremiseHostingControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private OnPremiseHostingController _controller;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OnPremiseHostingController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateOnPremiseCommand>(),
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
        public async Task PopulatedOnPremiseShouldReturnCorrectDetails(string summary, string link, string hostingModel, string requiredHscn)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.OnPremise == Mock.Of<IOnPremise>(p =>
                                                         p.Summary == summary &&
                                                         p.Link == link &&
                                                         p.HostingModel == hostingModel &&
                                                         p.RequiresHSCN == requiredHscn)));
            var result = await _controller.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetOnPremiseResult>();

            var onPremiseResult = result.Value as GetOnPremiseResult;
            onPremiseResult.Summary.Should().Be(summary);
            onPremiseResult.Link.Should().Be(link);
            onPremiseResult.HostingModel.Should().Be(hostingModel);

            if (requiredHscn == null)
            {
                onPremiseResult.RequiresHSCN.Should().BeEmpty();
            }
            else
            {
                onPremiseResult.RequiresHSCN.Should().BeEquivalentTo(requiredHscn);
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
            result.Value.Should().BeOfType<GetOnPremiseResult>();

            var onPremiseResult = result.Value as GetOnPremiseResult;
            onPremiseResult.Summary.Should().BeNull();
            onPremiseResult.Link.Should().BeNull();
            onPremiseResult.HostingModel.Should().BeNull();
            onPremiseResult.RequiresHSCN.Should().BeEmpty();
        }


        [Test]
        public async Task UpdateOnPremiseDetails()
        {
            var request = new UpdateOnPremiseViewModel();

            var result =
                (await _controller.Update(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateOnPremiseCommand>(c =>
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

            var request = new UpdateOnPremiseViewModel();

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
                    It.Is<UpdateOnPremiseCommand>(c =>
                        c.Id == _solutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

