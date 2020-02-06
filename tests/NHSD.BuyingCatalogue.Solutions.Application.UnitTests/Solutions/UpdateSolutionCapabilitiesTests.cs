using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public sealed class UpdateSolutionCapabilitiesTests
    {
        private TestContext _context;

        private const string ValidSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _context.MockSolutionRepository.Setup(x => x.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateCapabilitiesAsync()
        {
            var capabilityRefs = new HashSet<string>(){"C1", "C2"};

            var validationResult = await UpdateCapabilitiesAsync(ValidSolutionId, capabilityRefs).ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            _context.MockSolutionRepository.Verify(r => r.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionCapabilityRepository.Verify(
                r => r.UpdateCapabilitiesAsync(
                    It.Is<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == ValidSolutionId && c.NewCapabilitiesReference == capabilityRefs),
                    It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateCapabilitiesAsync(InvalidSolutionId, new HashSet<string>()));
        }

        private async Task<ISimpleResult> UpdateCapabilitiesAsync(string solutionId, HashSet<string> capabilityRefs)
        {
            return await _context.UpdateCapabilitiesHandler
                .Handle(new UpdateCapabilitiesCommand(solutionId, capabilityRefs), new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
