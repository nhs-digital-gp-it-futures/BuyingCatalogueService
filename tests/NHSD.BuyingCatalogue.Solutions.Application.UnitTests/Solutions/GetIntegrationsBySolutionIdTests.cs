using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetIntegrationsBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class GetIntegrationsBySolutionIdTests
    {
        private const string SolutionId = "Sln1";

        private TestContext context;
        private GetIntegrationsBySolutionIdQuery query;
        private CancellationToken cancellationToken;
        private string integrationsUrl = "Some integrations url";
        private Mock<IIntegrationsResult> mockResult;
        private bool solutionExists = true;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
            query = new GetIntegrationsBySolutionIdQuery(SolutionId);
            cancellationToken = CancellationToken.None;

            context.MockSolutionRepository
                .Setup(r => r.GetIntegrationsBySolutionIdAsync(SolutionId, cancellationToken))
                .ReturnsAsync(() => mockResult.Object);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, cancellationToken))
                .ReturnsAsync(() => solutionExists);

            mockResult = new Mock<IIntegrationsResult>();
            mockResult.Setup(m => m.IntegrationsUrl).Returns(() => integrationsUrl);
        }

        [TestCase("Some url")]
        [TestCase("         ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldGetIntegrationsUrl(string url)
        {
            integrationsUrl = url;
            var result = await context.GetIntegrationsBySolutionIdHandler.Handle(query, cancellationToken);
            result.Url.Should().Be(integrationsUrl);
        }

        [Test]
        public async Task EmptyIntegrationsResultReturnsDefaultIntegration()
        {
            integrationsUrl = null;

            var integrations = await context.GetIntegrationsBySolutionIdHandler.Handle(
                new GetIntegrationsBySolutionIdQuery(SolutionId),
                cancellationToken);

            integrations.Should().NotBeNull();
            integrations.Should().BeEquivalentTo(new IntegrationsDto());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            solutionExists = false;

            Assert.ThrowsAsync<NotFoundException>(() => context.GetIntegrationsBySolutionIdHandler.Handle(
                query,
                cancellationToken));

            context.MockSolutionRepository.Verify(r => r.CheckExists(SolutionId, cancellationToken));
            context.MockSolutionRepository.VerifyNoOtherCalls();
        }
    }
}
