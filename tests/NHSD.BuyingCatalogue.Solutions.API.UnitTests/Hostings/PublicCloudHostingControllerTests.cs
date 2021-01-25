using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    internal sealed class PublicCloudHostingControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private PublicCloudHostingController publicCloudHostingController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            publicCloudHostingController = new PublicCloudHostingController(mediatorMock.Object);

            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePublicCloudCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
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
            Expression<Func<IPublicCloud, bool>> publicCloud = p =>
                p.Summary == summary
                && p.Link == link
                && p.RequiresHSCN == requiresHscn;

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetHostingBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.PublicCloud == Mock.Of(publicCloud)));

            var result = await publicCloudHostingController.GetPublicCloudHosting(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetPublicCloudResult>();

            var publicCloudData = result.Value as GetPublicCloudResult;

            Assert.NotNull(publicCloudData);
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
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetHostingBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IHosting);

            var result = await publicCloudHostingController.GetPublicCloudHosting(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetPublicCloudResult>();

            var publicCloudData = result.Value as GetPublicCloudResult;

            Assert.NotNull(publicCloudData);
            publicCloudData.Summary.Should().BeNull();
            publicCloudData.Link.Should().BeNull();
            publicCloudData.RequiredHscn.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateValidPublicCloud()
        {
            var request = new UpdatePublicCloudViewModel
            {
                Summary = "New Summary",
                Link = "New URL",
                RequiresHSCNArray = new HashSet<string> { "New requires HSCN" },
            };

            var result = await publicCloudHostingController.UpdatePublicCloudHosting(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdatePublicCloudCommand>(c => c.SolutionId == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("summary", "maxLength");
            resultDictionary.Add("link", "maxLength");
            var request = new UpdatePublicCloudViewModel();

            var result = await publicCloudHostingController.UpdatePublicCloudHosting(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(2);
            validationResult["summary"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");

            Expression<Func<UpdatePublicCloudCommand, bool>> match = c =>
                c.Data.Summary == request.Summary
                && c.Data.Link == request.Link
                && c.Data.RequiresHSCN == request.RequiresHSCN
                && c.SolutionId == SolutionId;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
