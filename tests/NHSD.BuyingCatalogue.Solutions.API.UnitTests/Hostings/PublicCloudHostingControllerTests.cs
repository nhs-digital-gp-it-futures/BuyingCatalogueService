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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Hostings
{
    [TestFixture]
    public sealed class PublicCloudHostingControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private PublicCloudHostingController _publicCloudHostingController;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _publicCloudHostingController = new PublicCloudHostingController(_mediatorMock.Object);

            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdatePublicCloudCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [TestCase("Some summary", "Some link", "'Tis required")]
        [TestCase("Some summary", null, "'Tis required")]
        [TestCase("Some summary", null, null)]
        [TestCase(null, "Some link", "'Tis required")]
        [TestCase(null, "Some link", null)]
        [TestCase("Some summary", null, "'Tis required")]
        [TestCase("Some summary", "Some link", null)]
        [TestCase(null, null, "'Tis required")]
        [TestCase(null, null, null)]

        public async Task PopulatedPublicCloudShouldReturnCorrectPublicCloud(string summary, string link, string requiresHscn)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.PublicCloud == Mock.Of<IPublicCloud>(p =>
                                                         p.Summary == summary &&
                                                         p.Link == link &&
                                                         p.RequiresHSCN == requiresHscn)));
            var result = await _publicCloudHostingController.GetPublicCloudHosting(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetPublicCloudResult>();

            var publicCloudData = result.Value as GetPublicCloudResult;
            publicCloudData.Summary.Should().Be(summary);
            publicCloudData.Link.Should().Be(link);

            if (requiresHscn == null)
            {
                publicCloudData.RequiredHscn.Should().BeEmpty();
            }
            else
            {
                publicCloudData.RequiredHscn.Should().BeEquivalentTo(requiresHscn);
            }
        }

        [Test]
        public async Task NullHostingShouldReturnNull()
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IHosting);

            var result = await _publicCloudHostingController.GetPublicCloudHosting(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetPublicCloudResult>();

            var publicCloudData = result.Value as GetPublicCloudResult;
            publicCloudData.Summary.Should().BeNull();
            publicCloudData.Link.Should().BeNull();
            publicCloudData.RequiredHscn.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateValidPublicCloud()
        {
            var request = new UpdatePublicCloudViewModel()
            {
                Summary = "New Summary",
                Link = "New URL",
                RequiresHSCNArray = new HashSet<string> {"New requires HSCN"},
            };

            var result =
                (await _publicCloudHostingController.UpdatePublicCloudHosting(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdatePublicCloudCommand>(c =>
                        c.SolutionId == _solutionId &&
                        c.Data == request),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("summary", "maxLength");
            _resultDictionary.Add("link", "maxLength");
            var request = new UpdatePublicCloudViewModel();

            var result =
                (await _publicCloudHostingController.UpdatePublicCloudHosting(_solutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["summary"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");

            _mediatorMock.Verify(x => x.Send(
                    It.Is<UpdatePublicCloudCommand>(c =>
                        c.Data.Summary == request.Summary &&
                        c.Data.Link == request.Link &&
                        c.Data.RequiresHSCN == request.RequiresHSCN &&
                        c.SolutionId == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
