using System.Collections.Generic;
using System.Linq;
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
        private TestContext _context;

        private List<string> _solutionIds = new List<string>();
        private List<IAdditionalServiceResult> _additionalServiceResult;

        private static readonly IAdditionalServiceResult AdditionalService1 = Mock.Of<IAdditionalServiceResult>(a =>
            a.CatalogueItemId == "cat1" &&
            a.SolutionId == "Sln1" &&
            a.CatalogueItemName == "cat name" &&
            a.SolutionName == "solution 1 name");

        private static readonly IAdditionalServiceResult AdditionalService2 = Mock.Of<IAdditionalServiceResult>(a =>
            a.CatalogueItemId == "cat2" &&
            a.SolutionId == "Sln2" &&
            a.CatalogueItemName == "cat name 2" &&
            a.SolutionName == "solution 2 name");

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();

            _context.MockAdditionalServiceRepository
                .Setup(r => r.GetAdditionalServiceBySolutionIdsAsync(_solutionIds, new CancellationToken()))
                .ReturnsAsync(() => _additionalServiceResult);

            _additionalServiceResult = new List<IAdditionalServiceResult>();
        }

        [Test]
        public async Task EmptyAdditionalServiceResultReturnsDefaultAdditionalService()
        {
            var additionalService =
                await _context.GetAdditionalServiceBySolutionIdsHandler.Handle(
                    new GetAdditionalServiceBySolutionIdsQuery(_solutionIds), new CancellationToken());

            additionalService.Should().BeEmpty();
        }

        [Test]
        public async Task MultipleAdditionalServices_ReturnsResult()
        {
            _additionalServiceResult.Add(AdditionalService1);
            _additionalServiceResult.Add(AdditionalService2);

            _solutionIds.Add(AdditionalService1.SolutionId);
            _solutionIds.Add(AdditionalService2.SolutionId);

            var additionalService =
                (await _context.GetAdditionalServiceBySolutionIdsHandler.Handle(
                    new GetAdditionalServiceBySolutionIdsQuery(_solutionIds), new CancellationToken())).ToList();

            additionalService.Count().Should().Be(2);

            additionalService[0].SolutionId.Should().BeEquivalentTo(AdditionalService1.SolutionId);
            additionalService[1].SolutionId.Should().BeEquivalentTo(AdditionalService2.SolutionId);
        }
    }
}
