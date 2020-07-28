using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.AdditionalService;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class AdditionalServiceControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private AdditionalServiceController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new AdditionalServiceController(_mockMediator.Object);
        }

        [Test]
        public void Constructor_MediatorIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new AdditionalServiceController(null);
            });
        }

        [Test]
        public async Task GetAsync_NoSolutionIds_ReturnsNotFound()
        {
            var response = await _controller.GetAsync(null);
            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetAsync_SolutionDoesNotExist_ReturnsEmptyList()
        {
            var actual = (await _controller.GetAsync(new List<string> { "INVALID", "ANOTHER INVALID ID" })).Value;

            var expected = new AdditionalServiceListResult
            {
                AdditionalServices = new List<AdditionalServiceModel>()
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_SingleSolutionExists_ReturnsAdditionalService()
        {
            const string solutionId = "sln1";

            var additionalService1 = AdditionalServiceDtoBuilder.Create().WithSolutionId(solutionId).Build();
            IEnumerable<IAdditionalService> additionalServiceResult = new List<IAdditionalService> { additionalService1 };

            _mockMediator
                .Setup(m => m.Send(
                    It.IsNotNull<GetAdditionalServiceBySolutionIdsQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(() => additionalServiceResult);

            IEnumerable<string> solutionIds = new List<string> { solutionId };
            var response = await _controller.GetAsync(solutionIds);

            var expected = GetAdditionalServicesResult(additionalServiceResult);
            response.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAsync_MultipleSolutionsExist_ReturnsMultipleAdditionalServices()
        {
            const string solutionId1 = "sln1";
            const string solutionId2 = "sln2";

            var additionalService1 = AdditionalServiceDtoBuilder.Create().WithSolutionId(solutionId1).Build();
            var additionalService2 = AdditionalServiceDtoBuilder.Create().WithSolutionId(solutionId2).Build();

            List<IAdditionalService> additionalServiceResult = new List<IAdditionalService> { additionalService1, additionalService2 };

            _mockMediator
                .Setup(m => m.Send(
                    It.IsNotNull<GetAdditionalServiceBySolutionIdsQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(additionalServiceResult);

            var solutionIds = new List<string> { solutionId1, solutionId2 };
            var response = await _controller.GetAsync(solutionIds);

            var expected = GetAdditionalServicesResult(additionalServiceResult);
            response.Value.Should().BeEquivalentTo(expected);
        }

        private AdditionalServiceListResult GetAdditionalServicesResult(IEnumerable<IAdditionalService> additionalServices)
        {
            return new AdditionalServiceListResult
            {
                AdditionalServices = additionalServices.Select(x => new AdditionalServiceModel
                {
                    CatalogueItemId = x.CatalogueItemId,
                    Name = x.CatalogueItemName,
                    Summary = x.Summary,
                    Solution = new AdditionalServiceSolutionModel
                    {
                        SolutionId = x.SolutionId, Name = x.SolutionName
                    }
                })
            };
        }
    }
}
