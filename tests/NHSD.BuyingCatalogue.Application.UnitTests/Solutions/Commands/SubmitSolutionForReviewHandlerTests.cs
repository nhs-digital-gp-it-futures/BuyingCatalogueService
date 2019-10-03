using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Commands
{
    [TestFixture]
    public class SubmitSolutionForReviewHandlerTests
    {
        private Mock<ISolutionRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<ISolutionRepository>();
        }

        [Test]
        public async Task Handle_CallsRepositoryMethodByIdAsync_Once()
        {
            //ARRANGE
            var expectedSolutionId = Guid.NewGuid().ToString();
            var command = new SubmitSolutionForReviewCommand(expectedSolutionId);

            _repository.Setup(x => x.ByIdAsync(expectedSolutionId, CancellationToken.None)).ReturnsAsync(() => SolutionTestData.Default(expectedSolutionId));

            var testObject = new SubmitSolutionForReviewHandler(_repository.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(expectedSolutionId, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsRepositoryMethodUpdateSupplierStatusAsync_Once()
        {
            //ARRANGE
            var expectedSolutionId = Guid.NewGuid().ToString();
            var expectedSolution = SolutionTestData.Default(expectedSolutionId);
            var expectedSupplierStatus = SupplierStatus.AuthorityReview;

            var command = new SubmitSolutionForReviewCommand(expectedSolutionId);

            _repository.Setup(x => x.ByIdAsync(expectedSolutionId, CancellationToken.None)).ReturnsAsync(() => expectedSolution);
            _repository.Setup(x => x.UpdateSupplierStatusAsync(
                It.Is<Solution>(solution => expectedSolution.Equals(solution)),
                It.Is<SupplierStatus>(status => status.Equals(expectedSupplierStatus)), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var testObject = new SubmitSolutionForReviewHandler(_repository.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.UpdateSupplierStatusAsync(
                It.Is<Solution>(solution => expectedSolution.Equals(solution)),
                It.Is<SupplierStatus>(status => status.Equals(expectedSupplierStatus)), CancellationToken.None), Times.Once);
        }
    }
}
