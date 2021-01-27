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
using NHSD.BuyingCatalogue.Solutions.API.Controllers.Hosting;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.HybridHostingType;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Hosting
{
    [TestFixture]
    internal sealed class HybridHostingTypeControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private HybridHostingTypeController hybridHostingTypeController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            hybridHostingTypeController = new HybridHostingTypeController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateHybridHostingTypeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
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
        public async Task ShouldReturnCorrectHybridHostingTypeResultWhenHybridHostingTypeIsPopulated(
            string summary,
            string link,
            string hostingModel,
            string requiresHscn)
        {
            Expression<Func<IHybridHostingType, bool>> hybridHostingType = h =>
                h.Summary == summary
                && h.Link == link
                && h.HostingModel == hostingModel
                && h.RequiresHscn == requiresHscn;

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetHostingBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.HybridHostingType == Mock.Of(hybridHostingType)));

            var response = await hybridHostingTypeController.Get(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Value.Should().BeOfType<GetHybridHostingTypeResult>();

            var result = response.Value as GetHybridHostingTypeResult;

            Assert.NotNull(result);
            result.Summary.Should().Be(summary);
            result.Link.Should().Be(link);
            result.HostingModel.Should().Be(hostingModel);

            if (requiresHscn == null)
            {
                result.RequiresHscn.Should().BeEmpty();
            }
            else
            {
                result.RequiresHscn.Should().BeEquivalentTo(requiresHscn);
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

            var response = await hybridHostingTypeController.Get(SolutionId) as ObjectResult;

            Assert.NotNull(response);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Value.Should().BeOfType<GetHybridHostingTypeResult>();

            var result = response.Value as GetHybridHostingTypeResult;

            Assert.NotNull(result);
            result.Summary.Should().BeNull();
            result.Link.Should().BeNull();
            result.HostingModel.Should().BeNull();
            result.RequiresHscn.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateHybridHostingTypeDetails()
        {
            var request = new UpdateHybridHostingTypeViewModel();

            var result = await hybridHostingTypeController.Update(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateHybridHostingTypeCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));

            mediatorMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("summary", "maxLength");
            resultDictionary.Add("link", "maxLength");
            resultDictionary.Add("hosting-model", "maxLength");

            var request = new UpdateHybridHostingTypeViewModel();

            var result = await hybridHostingTypeController.Update(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(3);
            validationResult["summary"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");
            validationResult["hosting-model"].Should().Be("maxLength");

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateHybridHostingTypeCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
