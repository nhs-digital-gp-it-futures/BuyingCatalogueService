using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class AdditionalServiceGetBySolutionIdsTests
    {
        private static readonly Expression<Func<IAdditionalServiceResult, bool>> ServiceResult1 = a =>
            a.CatalogueItemId == "cat1"
            && a.SolutionId == "Sln1"
            && a.CatalogueItemName == "cat name"
            && a.SolutionName == "solution 1 name";

        private static readonly IAdditionalServiceResult AdditionalService1 = Mock.Of(ServiceResult1);

        private static readonly Expression<Func<IAdditionalServiceResult, bool>> ServiceResult2 = a =>
            a.CatalogueItemId == "cat2"
            && a.SolutionId == "Sln2"
            && a.CatalogueItemName == "cat name 2"
            && a.SolutionName == "solution 2 name";

        private static readonly IAdditionalServiceResult AdditionalService2 = Mock.Of(ServiceResult2);

        private readonly List<string> solutionIds = new();

        private TestContext context;
        private List<IAdditionalServiceResult> additionalServiceResult;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();

            context.MockAdditionalServiceRepository
                .Setup(r => r.GetAdditionalServiceBySolutionIdsAsync(solutionIds, CancellationToken.None))
                .ReturnsAsync(() => additionalServiceResult);

            additionalServiceResult = new List<IAdditionalServiceResult>();
        }

        [Test]
        public async Task EmptyAdditionalServiceResultReturnsDefaultAdditionalService()
        {
            var additionalService = await context.GetAdditionalServiceBySolutionIdsHandler.Handle(
                new GetAdditionalServiceBySolutionIdsQuery(solutionIds),
                CancellationToken.None);

            additionalService.Should().BeEmpty();
        }

        [Test]
        public async Task MultipleAdditionalServices_ReturnsResult()
        {
            additionalServiceResult.Add(AdditionalService1);
            additionalServiceResult.Add(AdditionalService2);

            solutionIds.Add(AdditionalService1.SolutionId);
            solutionIds.Add(AdditionalService2.SolutionId);

            var additionalService = (await context.GetAdditionalServiceBySolutionIdsHandler.Handle(
                    new GetAdditionalServiceBySolutionIdsQuery(solutionIds),
                    CancellationToken.None)).ToList();

            additionalService.Count.Should().Be(2);

            additionalService[0].SolutionId.Should().BeEquivalentTo(AdditionalService1.SolutionId);
            additionalService[1].SolutionId.Should().BeEquivalentTo(AdditionalService2.SolutionId);
        }
    }
}
