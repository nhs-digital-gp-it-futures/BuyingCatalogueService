using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Hostings;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings;
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

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _publicCloudHostingController = new PublicCloudHostingController(_mediatorMock.Object);
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

        public async Task PopulatedPublicCloudShouldReturnCorrectPublicCloud(string summary, string url, string connectivityRequired)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.PublicCloud == Mock.Of<IPublicCloud>(p =>
                                                         p.Summary == summary &&
                                                         p.URL == url &&
                                                         p.ConnectivityRequired == connectivityRequired)));
            var result = await _publicCloudHostingController.GetPublicCloudHosting(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetPublicCloudResult>();

            var publicCloudData = result.Value as GetPublicCloudResult;
            publicCloudData.Summary.Should().Be(summary);
            publicCloudData.URL.Should().Be(url);

            if (connectivityRequired == null) 
            {
                publicCloudData.ConnectivityRequired.Should().BeEmpty();
            }
            else
            {
                publicCloudData.ConnectivityRequired.Should().BeEquivalentTo(connectivityRequired);
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
            publicCloudData.URL.Should().BeNull();
            publicCloudData.ConnectivityRequired.Should().BeEmpty();
        }
    }
}
