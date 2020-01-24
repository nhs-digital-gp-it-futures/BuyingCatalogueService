using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSuppliers;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.Suppliers
{
    [TestFixture]
    internal sealed class UpdateSupplierTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IUpdateSupplierData> _dataMock;
        private string _description;
        private string _link;

        private TestContext _context;

        [SetUp]
        public void Setup()
        {
            _description = "Some Description";
            _link = "A URL";

            _context = new TestContext();

            _dataMock = new Mock<IUpdateSupplierData>();
            _dataMock.Setup(x => x.Description).Returns(() => _description);
            _dataMock.Setup(x => x.Link).Returns(() => _link);
        }

        [Test]
        public async Task ShouldUpdateSupplierOnHandler()
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSupplierRepositoryGetByIdAsync();

            var validationResult = await UpdateSupplier().ConfigureAwait(false);

            _context.MockSupplierRepository.Verify(s =>
                s.UpdateSupplierAsync(
                    It.Is<IUpdateSupplierRequest>(r =>
                        r.SolutionId == SolutionId && r.Description == _description && r.Link == _link),
                    CancellationToken.None), Times.Once);
            _context.MockSupplierRepository.Verify();
            _context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSupplierInformationToNullOnHandler()
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSupplierRepositoryGetByIdAsync();

            _description = null;
            _link = null;

            var validationResult = await UpdateSupplier().ConfigureAwait(false);

            _context.MockSupplierRepository.Verify(s =>
                s.UpdateSupplierAsync(
                    It.Is<IUpdateSupplierRequest>(r =>
                        r.SolutionId == SolutionId && r.Description == _description && r.Link == _link),
                    CancellationToken.None), Times.Once);
            _context.MockSupplierRepository.Verify();
            _context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeTrue();
        }

        [TestCase(1001, 1000, "description")]
        [TestCase(1000, 1001, "link")]
        [TestCase(1001, 1001, "description", "link")]
        public async Task ShouldNotUpdateInvalidSupplierOnHandler(int description, int link, params string[] expected)
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSupplierRepositoryGetByIdAsync();

            _description = new string('a', description);
            _link = new string('a', link);

            var validationResult = await UpdateSupplier().ConfigureAwait(false);

            _context.MockSupplierRepository.Verify(s =>
                s.UpdateSupplierAsync(
                    It.Is<IUpdateSupplierRequest>(r =>
                        r.SolutionId == SolutionId && r.Description == _description && r.Link == _link),
                    CancellationToken.None), Times.Never);
            _context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<MaxLengthResult>();
            var maxLengthResult = validationResult as MaxLengthResult;
            maxLengthResult.MaxLength.Should().BeEquivalentTo(expected);
        }

        private async Task<ISimpleResult> UpdateSupplier()
        {
            return await _context.UpdateSupplierHandler.Handle(new UpdateSupplierCommand(SolutionId, _dataMock.Object),
                new CancellationToken()).ConfigureAwait(false);
        }

        private void SetupMockSolutionRepositoryGetByIdAsync(bool solutionExists = true)
        {
            var existingSolution = new Mock<ISolutionResult>();

            existingSolution.Setup(s => s.Id).Returns(SolutionId);

            _context.MockSolutionRepository.Setup(x => x.CheckExists(SolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(solutionExists);

            _context.MockSolutionRepository.Setup(s => s.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object).Verifiable();
        }

        private void SetupMockSupplierRepositoryGetByIdAsync()
        {
            var existingSupplier = new Mock<ISupplierResult>();
            var request = new Mock<IUpdateSupplierRequest>();

            existingSupplier.Setup(s => s.SolutionId).Returns(SolutionId);

            _context.MockSupplierRepository.Setup(s => s.GetSupplierBySolutionIdAsync(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSupplier.Object).Verifiable();
        }
    }
}
