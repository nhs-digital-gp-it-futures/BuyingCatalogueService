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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Hosting.OnPremise;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Hosting
{
    [TestFixture]
    internal sealed class OnPremiseHostingControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private OnPremiseHostingController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new OnPremiseHostingController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateOnPremiseCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
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
        public async Task PopulatedOnPremiseShouldReturnCorrectDetails(
            string summary,
            string link,
            string hostingModel,
            string requiredHscn)
        {
            Expression<Func<IOnPremise, bool>> onPremise = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hostingModel
                && p.RequiresHscn == requiredHscn;

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetHostingBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IHosting>(h => h.OnPremise == Mock.Of(onPremise)));

            var result = await controller.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetOnPremiseResult>();

            var onPremiseResult = result.Value as GetOnPremiseResult;

            Assert.NotNull(onPremiseResult);
            onPremiseResult.Summary.Should().Be(summary);
            onPremiseResult.Link.Should().Be(link);
            onPremiseResult.HostingModel.Should().Be(hostingModel);

            if (requiredHscn == null)
            {
                onPremiseResult.RequiresHscn.Should().BeEmpty();
            }
            else
            {
                onPremiseResult.RequiresHscn.Should().BeEquivalentTo(requiredHscn);
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

            var result = await controller.Get(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetOnPremiseResult>();

            var onPremiseResult = result.Value as GetOnPremiseResult;

            Assert.NotNull(onPremiseResult);
            onPremiseResult.Summary.Should().BeNull();
            onPremiseResult.Link.Should().BeNull();
            onPremiseResult.HostingModel.Should().BeNull();
            onPremiseResult.RequiresHscn.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateOnPremiseDetails()
        {
            var request = new UpdateOnPremiseViewModel();

            var result = await controller.Update(SolutionId, request) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateOnPremiseCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("summary", "maxLength");
            resultDictionary.Add("link", "maxLength");
            resultDictionary.Add("hosting-model", "maxLength");

            var request = new UpdateOnPremiseViewModel();

            var result = await controller.Update(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(3);
            validationResult["summary"].Should().Be("maxLength");
            validationResult["link"].Should().Be("maxLength");
            validationResult["hosting-model"].Should().Be("maxLength");

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateOnPremiseCommand>(c => c.Id == SolutionId && c.Data == request),
                It.IsAny<CancellationToken>()));
        }
    }
}
