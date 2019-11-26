using System.Threading;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    internal class ClientApplicationTestsBase
    {
        protected TestContext Context;

        [SetUp]
        public void SetUpFixture()
        {
            Context = new TestContext();
        }

        protected void SetUpMockSolutionRepositoryGetByIdAsync(string clientApplicationJson)
        {
            var existingSolution = new Mock<ISolutionResult>();

            existingSolution.Setup(s => s.Id).Returns("Sln1");

            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplicationJson);

            Context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
        }
    }
}
