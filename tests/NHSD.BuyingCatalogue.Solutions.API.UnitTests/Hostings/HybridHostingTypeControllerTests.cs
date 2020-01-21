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
    public sealed class HybridHostingTypeControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private HybridHostingTypeController _hybridHostingTypeController;
        private readonly string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _hybridHostingTypeController = new HybridHostingTypeController(_mediatorMock.Object);
        }

        [TestCase("Some summary", "Some link", "Some hosting model", "'Tis required")]
        [TestCase(null, "Some link", "Some hosting model", "'Tis required")]
        [TestCase("Some summary", null, "Some hosting model", "'Tis required")]
        [TestCase("Some summary", "Some link", null, "'Tis required")]
        [TestCase("Some summary", "Some link", "Some hosting model", null)]
        [TestCase("Some summary", "Some link", null, null)]
        [TestCase("Some summary", null, null, "'Tis required")]
        [TestCase("Some summary", null, "Some hosting model", null)]
        [TestCase(null, "Some link", "Some hosting model", null)]
        [TestCase(null, "Some link", null, "'Tis required")]
        [TestCase("Some summary", null, null, null)]
        [TestCase(null, "Some link", null, null)]
        [TestCase(null, null, "Some hosting model", null)]
        [TestCase(null, null, null, "'Tis required")]
        [TestCase(null, null, null, null)]
        public async Task ShouldReturnCorrectHybridHostingTypeResultWhenHybridHostingTypeIsPopulated(string summary, string link, string hostingModel, string requiresHSCN)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.HybridHostingType == Mock.Of<IHybridHostingType>(p =>
                                                         p.Summary == summary
                                                         && p.Link == link
                                                         && p.HostingModel == hostingModel
                                                         && p.RequiresHSCN == requiresHSCN)));

            var response = await _hybridHostingTypeController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Value.Should().BeOfType<GetHybridHostingTypeResult>();

            var result = response.Value as GetHybridHostingTypeResult;
            result.Should().NotBeNull();
            result.Summary.Should().Be(summary);
            result.Link.Should().Be(link);
            result.HostingModel.Should().Be(hostingModel);

            if (requiresHSCN == null)
            {
                result.RequiresHSCN.Should().BeEmpty();
            }
            else
            {
                result.RequiresHSCN.Should().BeEquivalentTo(requiresHSCN);
            }
        }

        [Test]
        public async Task NullHostingShouldReturnNull()
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetHostingBySolutionIdQuery>(q => q.Id == _solutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IHosting);

            var response = await _hybridHostingTypeController.Get(_solutionId).ConfigureAwait(false) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Value.Should().BeOfType<GetHybridHostingTypeResult>();

            var result = response.Value as GetHybridHostingTypeResult;
            result.Should().NotBeNull();
            result.Summary.Should().BeNull();
            result.Link.Should().BeNull();
            result.HostingModel.Should().BeNull();
            result.RequiresHSCN.Should().BeEmpty();
        }
    }
}
