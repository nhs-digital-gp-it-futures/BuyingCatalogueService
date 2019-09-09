using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Domain;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Commands
{
    [TestFixture]
    public sealed class UpdateSolutionHandlerTests
    {
        private Mock<ISolutionRepository> _repository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<ISolutionRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Handle_CallsRepository_Twice()
        {
            //ARRANGE
            var expectedSolutionId = Guid.NewGuid().ToString();
            var testData = SolutionTestData.Default(expectedSolutionId);
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = "Update" });

            var updatedSolution = SolutionTestData.Default(expectedSolutionId);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData;

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, testData)).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => testData);
            _repository.Setup(x => x.UpdateAsync(It.Is<Solution>(solution => updatedSolution.Equals(solution)), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new UpdateSolutionHandler(_repository.Object, _mapper.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(testData.Id, CancellationToken.None), Times.Once);
            _repository.Verify(x => x.UpdateAsync(It.Is<Solution>(solution => updatedSolution.Equals(solution)), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var expectedSolutionId = Guid.NewGuid().ToString();
            var testData = SolutionTestData.Default(expectedSolutionId);
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = "Update" });

            var updatedSolution = SolutionTestData.Default(expectedSolutionId);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData;

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, testData)).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => testData);
            _repository.Setup(x => x.UpdateAsync(It.Is<Solution>(solution => updatedSolution.Equals(solution)), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new UpdateSolutionHandler(_repository.Object, _mapper.Object);

            //ACT
            _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map(command.UpdateSolutionViewModel, testData), Times.Once);
        }

        [Test]
        public void Handle_SolutionNotFound_ExceptionRaised()
        {
            //ARRANGE
            var expectedSolutionId = Guid.NewGuid().ToString();
            var testData = SolutionTestData.Default(expectedSolutionId);
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = "Update" });

            var updatedSolution = SolutionTestData.Default(expectedSolutionId);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData;

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, testData)).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => null);

            var testObject = new UpdateSolutionHandler(_repository.Object, _mapper.Object);

            //ACT & ASSERT
            var actual = Assert.ThrowsAsync<NotFoundException>(async () => await testObject.Handle(command, CancellationToken.None));

            actual.ShouldNotBeNull();
            actual.Message.ShouldBe<string>($"Entity named '{nameof(Solution)}' could not be found matching the ID '{testData.Id}'.");
        }
    }
}
