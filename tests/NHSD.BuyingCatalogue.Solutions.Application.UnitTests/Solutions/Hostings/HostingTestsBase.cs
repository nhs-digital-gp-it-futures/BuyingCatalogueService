using System.Threading;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Hostings
{
    internal abstract class HostingTestsBase
    {
        protected TestContext Context;

        [SetUp]
        public void SetUpFixture()
        {
            Context = new TestContext();
        }

        protected void SetUpMockSolutionRepositoryGetByIdAsync(string hostingJson = "")
        {
            var existingSolution = new Mock<ISolutionResult>();

            existingSolution.Setup(s => s.Id).Returns("Sln1");

            existingSolution.Setup(s => s.Hosting).Returns(hostingJson);

            Context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);
        }
    }
}
