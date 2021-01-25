using System;
using System.Linq.Expressions;
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
    internal sealed class UpdateSolutionSupplierTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IUpdateSupplierData> dataMock;
        private string description;
        private string link;

        private TestContext context;

        [SetUp]
        public void Setup()
        {
            description = "Some Description";
            link = "A URL";

            context = new TestContext();

            dataMock = new Mock<IUpdateSupplierData>();
            dataMock.Setup(d => d.Description).Returns(() => description);
            dataMock.Setup(d => d.Link).Returns(() => link);
        }

        [Test]
        public async Task ShouldUpdateSupplierOnHandler()
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSolutionCheckExists();
            SetupMockSupplierRepositoryGetByIdAsync();

            var validationResult = await UpdateSupplier();

            Expression<Func<IUpdateSupplierRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && r.Description == description
                && r.Link == link;

            context.MockSupplierRepository.Verify(s => s.UpdateSupplierAsync(It.Is(match), CancellationToken.None));
            context.MockSupplierRepository.Verify();
            context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSupplierInformationToNullOnHandler()
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSolutionCheckExists();
            SetupMockSupplierRepositoryGetByIdAsync();

            description = null;
            link = null;

            var validationResult = await UpdateSupplier();

            Expression<Func<IUpdateSupplierRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && r.Description == description
                && r.Link == link;

            context.MockSupplierRepository.Verify(s => s.UpdateSupplierAsync(It.Is(match), CancellationToken.None));
            context.MockSupplierRepository.Verify();
            context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeTrue();
        }

        [TestCase(1101, 1000, "description")]
        [TestCase(1100, 1001, "link")]
        [TestCase(1101, 1001, "description", "link")]
        public async Task ShouldNotUpdateInvalidSupplierOnHandler(
            int requestDescription,
            int requestLink,
            params string[] expected)
        {
            SetupMockSolutionRepositoryGetByIdAsync();
            SetupMockSolutionCheckExists();
            SetupMockSupplierRepositoryGetByIdAsync();

            description = new string('a', requestDescription);
            link = new string('a', requestLink);

            var validationResult = await UpdateSupplier();

            Expression<Func<IUpdateSupplierRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && r.Description == description
                && r.Link == link;

            context.MockSupplierRepository.Verify(
                s => s.UpdateSupplierAsync(It.Is(match), CancellationToken.None),
                Times.Never());

            context.MockSupplierRepository.VerifyNoOtherCalls();

            validationResult.IsValid.Should().BeFalse();
            validationResult.Should().BeOfType<MaxLengthResult>();

            var maxLengthResult = validationResult as MaxLengthResult;

            Assert.NotNull(maxLengthResult);
            maxLengthResult.MaxLength.Should().BeEquivalentTo(expected);
        }

        private async Task<ISimpleResult> UpdateSupplier()
        {
            return await context.UpdateSolutionSupplierHandler.Handle(
                new UpdateSolutionSupplierCommand(SolutionId, dataMock.Object),
                new CancellationToken());
        }

        private void SetupMockSolutionCheckExists(bool solutionExists = true)
        {
            context.MockSolutionRepository
                .Setup(r => r.CheckExists(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(solutionExists);
        }

        private void SetupMockSolutionRepositoryGetByIdAsync()
        {
            var existingSolution = new Mock<ISolutionResult>();

            existingSolution.Setup(s => s.Id).Returns(SolutionId);

            context.MockSolutionRepository
                .Setup(s => s.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object).Verifiable();
        }

        private void SetupMockSupplierRepositoryGetByIdAsync()
        {
            var existingSupplier = new Mock<ISolutionSupplierResult>();

            existingSupplier.Setup(s => s.SolutionId).Returns(SolutionId);

            context.MockSupplierRepository
                .Setup(s => s.GetSupplierBySolutionIdAsync(SolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSupplier.Object).Verifiable();
        }
    }
}
