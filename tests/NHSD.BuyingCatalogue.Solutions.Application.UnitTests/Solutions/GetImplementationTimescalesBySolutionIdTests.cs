using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetImplementationTimescalesBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class GetImplementationTimescalesBySolutionIdTests
    {
        private const string SolutionId = "Sln1";

        private TestContext context;
        private GetImplementationTimescalesBySolutionIdQuery query;
        private CancellationToken cancellationToken;
        private string implementationTimescalesDescription = "Some implementation timescales description";
        private Mock<IImplementationTimescalesResult> mockResult;
        private bool solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            query = new GetImplementationTimescalesBySolutionIdQuery(SolutionId);
            cancellationToken = CancellationToken.None;

            context.MockSolutionDetailRepository
                .Setup(r => r.GetImplementationTimescalesBySolutionIdAsync(SolutionId, cancellationToken))
                .ReturnsAsync(() => mockResult.Object);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, cancellationToken))
                .ReturnsAsync(() => solutionExists);

            mockResult = new Mock<IImplementationTimescalesResult>();
            mockResult.Setup(m => m.Description).Returns(() => implementationTimescalesDescription);
        }

        [TestCase("Some description")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetImplementationTimescalesDescription(string description)
        {
            implementationTimescalesDescription = description;
            var result = await context.GetImplementationTimescalesBySolutionIdHandler.Handle(query, cancellationToken);
            result.Description.Should().Be(implementationTimescalesDescription);
        }

        [Test]
        public async Task EmptyImplementationTimescalesResultReturnsDefaultImplementationTimescales()
        {
            implementationTimescalesDescription = null;

            var implementationTimescales = await context.GetImplementationTimescalesBySolutionIdHandler.Handle(
                new GetImplementationTimescalesBySolutionIdQuery(SolutionId),
                cancellationToken);

            implementationTimescales.Should().NotBeNull();
            implementationTimescales.Should().BeEquivalentTo(new ImplementationTimescalesDto());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            solutionExists = false;

            Assert.ThrowsAsync<NotFoundException>(() => context.GetImplementationTimescalesBySolutionIdHandler.Handle(
                query,
                cancellationToken));

            context.MockSolutionRepository.Verify(r => r.CheckExists(SolutionId, cancellationToken));
            context.MockSolutionRepository.VerifyNoOtherCalls();
            context.MockSolutionDetailRepository.VerifyNoOtherCalls();
        }
    }
}
