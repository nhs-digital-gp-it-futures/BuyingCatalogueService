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
    public sealed class OnPremiseHostingControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private OnPremiseHostingController _controller;
        private readonly string _solutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OnPremiseHostingController(_mediatorMock.Object);
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
            var result = await _controller.GetPremiseHosting(_solutionId).ConfigureAwait(false) as ObjectResult;
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

            var result = await _controller.GetPremiseHosting(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetOnPremiseResult>();

            var onPremiseResult = result.Value as GetOnPremiseResult;
            onPremiseResult.Summary.Should().BeNull();
            onPremiseResult.Link.Should().BeNull();
            onPremiseResult.HostingModel.Should().BeNull();
            onPremiseResult.RequiresHSCN.Should().BeEmpty();
        }
    }
}

