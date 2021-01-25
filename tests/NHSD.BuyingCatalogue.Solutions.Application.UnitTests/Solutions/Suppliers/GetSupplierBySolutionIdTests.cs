using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Suppliers
{
    [TestFixture]
    internal sealed class GetSupplierBySolutionIdTests
    {
        private TestContext context;
        private string solutionId;
        private CancellationToken cancellationToken;
        private ISolutionSupplierResult solutionSupplierResult;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            solutionId = "Sln1";
            cancellationToken = CancellationToken.None;
            context.MockSupplierRepository
                .Setup(r => r.GetSupplierBySolutionIdAsync(solutionId, cancellationToken))
                .ReturnsAsync(() => solutionSupplierResult);
        }

        [Test]
        public async Task ShouldGetAboutSupplierById()
        {
            var originalSupplier = new SolutionSupplier
            {
                Name = "Some name",
                Summary = "Some Summary",
                Url = "Some Url",
            };

            Expression<Func<ISolutionSupplierResult, bool>> resultExpression = r =>
                r.SolutionId == solutionId
                && r.Name == originalSupplier.Name
                && r.Summary == originalSupplier.Summary
                && r.Url == originalSupplier.Url;

            solutionSupplierResult = Mock.Of(resultExpression);

            var newSupplier = await context.GetSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(solutionId),
                cancellationToken);

            newSupplier.Should().BeEquivalentTo(originalSupplier);
        }

        [Test]
        public async Task EmptySupplierResultReturnsDefaultSupplier()
        {
            Expression<Func<ISolutionSupplierResult, bool>> resultExpression = r =>
                r.SolutionId == solutionId
                && r.Name == null
                && r.Summary == null
                && r.Url == null;

            solutionSupplierResult = Mock.Of(resultExpression);

            var supplier = await context.GetSupplierBySolutionIdHandler.Handle(
                new GetSupplierBySolutionIdQuery(solutionId),
                cancellationToken);

            supplier.Should().NotBeNull();
            supplier.Should().BeEquivalentTo(new SolutionSupplierDto());
        }
    }
}
