using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Contracts.Persistence;
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

            var mockSolutionResult = new Mock<ISolutionResult>();
            mockSolutionResult.Setup(r => r.Id).Returns(expectedSolutionId);
            _repository.Setup(x => x.ByIdAsync(expectedSolutionId, CancellationToken.None)).ReturnsAsync(() => mockSolutionResult.Object);

            var testObject = new SubmitSolutionForReviewHandler(new SolutionReader(_repository.Object), _repository.Object);

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
            var expectedSupplierStatus = SupplierStatus.AuthorityReview;

            var command = new SubmitSolutionForReviewCommand(expectedSolutionId);

            var mockSolutionResult = new Mock<ISolutionResult>();
            mockSolutionResult.Setup(r => r.Id).Returns(expectedSolutionId);
            _repository.Setup(x => x.ByIdAsync(expectedSolutionId, CancellationToken.None)).ReturnsAsync(() => mockSolutionResult.Object);
            _repository.Setup(x => x.UpdateSupplierStatusAsync(It.IsAny<IUpdateSolutionSupplierStatusRequest>(), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new SubmitSolutionForReviewHandler(new SolutionReader(_repository.Object), _repository.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.UpdateSupplierStatusAsync(
                It.Is<UpdateSolutionSupplierStatusRequest>(r => r.Id.Equals(expectedSolutionId) && r.SupplierStatusId.Equals(expectedSupplierStatus.Id)),
                CancellationToken.None), Times.Once);
        }
    }
}
